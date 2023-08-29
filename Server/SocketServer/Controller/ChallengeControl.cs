using System;
using System.Collections.Generic;
using System.Text;
using SocketGameProtocol;
using SocketServer.DAO;

namespace SocketServer.Controller
{
    class ChallengeControl
    {
        private readonly int maxChallenges = 10;  //每个玩家的最大同时挑战数为10个
        public static ChallengePack[] ActiveChallenges;//当前所有进行中且未完成的挑战
        private Queue<int> freeIndex; //存储ActiveChallenges[]删除元素后的空闲元素下标
        public static Dictionary<int, int> challengeNum = new Dictionary<int, int>();//玩家ID与当前进行中的挑战数对应
        public UserData userData;
        public static ChallengeData challengeData;
        public UsersSongData usersSongData;
        public SongData songData;
        public ChallengeNoticeData challengeNoticeData;
        public ChallengeControl()
        {
            userData = new UserData();
            challengeData = new ChallengeData();
            usersSongData = new UsersSongData();
            songData = new SongData();
            challengeNoticeData = new ChallengeNoticeData();
            freeIndex = new Queue<int>();
            ChallengeInitialize();
        }

        public static void ChallengeInitialize()
        {
            ActiveChallenges = challengeData.GetAllUncompletedChallenges();
            if (ActiveChallenges!=null)
            {
                foreach (ChallengePack pack in ActiveChallenges)
                {
                    if (challengeNum.ContainsKey(pack.Challenger))
                        challengeNum[pack.Challenger]++;
                    else
                        challengeNum[pack.Challenger] = 1;
                }
            }
            
        }

        //发起挑战
        public MainPack SendChallenge(MainPack pack)
        {
            Console.WriteLine(pack.Challengepack);
            try
            {
                int challengerId = pack.Challengepack[0].Challenger;
                User challenger = userData.GetUser(challengerId);

                //检查该玩家先前发起过的进行中的挑战,在挑战全败的情况下,是否支付得起本次挑战的积分;
                //检查该玩家是否发起过未完成的相同乐曲的挑战,若存在,则不允许发起.
                double previousScore = 0;
                if (ActiveChallenges != null)
                {
                    foreach (ChallengePack challengePack in ActiveChallenges)
                    {
                        if (challengePack.Challenger == challengerId && challengePack.Completed != 1)
                        {
                            previousScore += challengePack.BaseScore * challengePack.Ratio;
                            if (challengePack.SongName == pack.Challengepack[0].SongName)
                            {
                                Console.WriteLine(challengerId + "发起相同歌曲的挑战");
                                pack.Returncode = ReturnCode.Fail;
                                return pack;
                            }
                        }
                    }
                }

                if (challenger.Scores >= previousScore + pack.Challengepack[0].Ratio * pack.Challengepack[0].BaseScore)
                {
                    challenger.Scores -= (int)pack.Challengepack[0].BaseScore;
                    userData.UpdateUser(challenger);

                }
                else
                {
                    Console.WriteLine(challengerId + "积分不足");
                    pack.Returncode = ReturnCode.Fail;
                    return pack;
                }
                if (challengeNum.ContainsKey(challengerId))
                {
                    pack.Challengepack[0].ChallengerName = challenger.Usrname;
                    if (challengeNum[challengerId] <= maxChallenges && challengeData.CreateChallenge(pack.Challengepack[0]))
                    {
                        if (freeIndex.Count > 0)
                            ActiveChallenges.SetValue(pack.Challengepack[0],freeIndex.Dequeue());
                        else
                        {
                            Array.Resize(ref ActiveChallenges, ActiveChallenges.Length+1);
                            ActiveChallenges[ActiveChallenges.Length - 1] = pack.Challengepack[0];
                        }
                        challengeNum[challengerId]++;
                        pack.Returncode = ReturnCode.Succeed;
                    }
                    else
                    {
                        Console.WriteLine(challengerId + "同时进行的挑战数过多");
                        pack.Returncode = ReturnCode.ChallengeTooMany;
                    }
                }
                else
                {
                    if (challengeData.CreateChallenge(pack.Challengepack[0]))
                    {
                        if (freeIndex.Count > 0)
                            ActiveChallenges.SetValue(pack.Challengepack[0], freeIndex.Dequeue());
                        else
                        {
                            if (ActiveChallenges != null)
                            {
                                Array.Resize(ref ActiveChallenges, ActiveChallenges.Length + 1);
                                ActiveChallenges[ActiveChallenges.Length - 1] = pack.Challengepack[0];
                            }
                            else
                            {
                                Array.Resize(ref ActiveChallenges, 1);
                                ActiveChallenges[ActiveChallenges.Length - 1] = pack.Challengepack[0];
                            }
                        }
                        challengeNum.Add(challengerId, 1);
                        pack.Returncode = ReturnCode.Succeed;
                    }
                    else
                    {
                        pack.Returncode = ReturnCode.Fail;
                    }
                }
                
                return pack;
            }
            catch(Exception e)
            {
                pack.Returncode = ReturnCode.Fail;
                Console.WriteLine(e.Message+"--SendChallenge");
                return pack;
            }
            
        }

        //获取所有玩家发起的挑战
        public MainPack GetAllChallenges(MainPack pack)
        {
            try
            {
                pack.Challengepack.Clear();
                if (ActiveChallenges != null)
                {
                    foreach (ChallengePack challengePack in ActiveChallenges)
                    {
                        if (challengePack.Completing == 0)
                            pack.Challengepack.Add(challengePack);
                    }
                }
                
                pack.Returncode = ReturnCode.Succeed;
                pack = GetScoreRecord(pack);
                pack = GetChallengerName(pack);
                return pack;
            }
            catch (Exception e)
            {
                pack.Returncode = ReturnCode.Fail;
                Console.WriteLine(e.Message+"--GetAllChallenges");
                return pack;
            }
        }

        //按条件搜索挑战
        public MainPack SearchChallenges(MainPack pack)
        {
            try
            {
                ChallengePack challengePack = pack.Challengepack[0];
                pack.Challengepack.Clear();
                if (challengePack.Challenger != 0 && challengeData.SearchChallengeByChallenger(challengePack.Challenger) != null)
                {
                    pack.Challengepack.Add(challengeData.SearchChallengeByChallenger(challengePack.Challenger));
                    if (pack.Challengepack != null)
                    {
                        pack.Returncode = ReturnCode.Succeed;
                    }
                    else
                    {
                        pack.Returncode = ReturnCode.Fail;
                    }
                    pack = GetScoreRecord(pack);
                    pack = GetChallengerName(pack);
                    return pack;
                }
                if ((challengePack.SongName == "") && (challengePack.Author == ""))
                {
                    return GetAllChallenges(pack);
                }
                else if ((challengePack.SongName == "") && (challengePack.Author != ""))
                {
                    pack.Challengepack.Add(challengeData.SearchChallengeByAuthor(challengePack.Author));
                }
                else if ((challengePack.SongName != "") && (challengePack.Author == ""))
                {
                    pack.Challengepack.Add(challengeData.SearchChallengeBySongname(challengePack.SongName));
                }
                else if ((challengePack.SongName != "") && (challengePack.Author != ""))
                {
                    pack.Challengepack.Add(challengeData.SearchChallengeByNameAndAuthor(challengePack.Author,challengePack.SongName));
                }
                if (pack.Challengepack != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }
                pack = GetScoreRecord(pack);
                pack = GetChallengerName(pack);
                return pack;
            }
            catch (Exception e)
            {
                pack.Returncode = ReturnCode.Fail;
                Console.WriteLine(e.Message+ "--SearchChallenges");
                return pack;
            }
        }
        public MainPack GetScoreRecord(MainPack pack)
        {
            pack.Userssong.Clear();
            for(int i = 0; i < pack.Challengepack.Count; i++)
            {
                pack.Userssong.Add(usersSongData.GetSongByUseridAndName(pack.User.Userid, pack.Challengepack[i].SongName));
            }
            return pack;
        }
        public MainPack GetChallengerName(MainPack pack)
        {
            for (int i = 0; i < pack.Challengepack.Count; i++)
            {
                pack.Challengepack[i].ChallengerName = userData.GetUser(pack.Challengepack[i].Challenger).Usrname;
            }
            return pack;
        }

        //应答挑战(在应答者应答后将Challenge的completing字段先置'1',以防其完成游戏期间被其他玩家应答)
        public MainPack AcceptChallenge(MainPack pack)
        {
            ChallengePack challengePack = pack.Challengepack[0];
            ChallengePack challenge = challengeData.GetUncompletedChallenge("SELECT * FROM Challenge WHERE challengeId=" +
                challengeData.FindChallenge(challengePack));
            User accepter = userData.GetUser(challengePack.Accepter);
            //检查该玩家先前发起过的进行中的挑战，在挑战全败的情况下，是否支付得起本次挑战的积分
            double previousScore = 0;
            if(ActiveChallenges != null)
            {
                foreach (ChallengePack chpk in ActiveChallenges)
                {
                    if (chpk.Challenger == accepter.Userid && chpk.Completed != 1)
                    {
                        previousScore += chpk.BaseScore * chpk.Ratio;
                    }
                }
            }
            
            //Console.WriteLine(challenge.ChallengeId);
            //Console.WriteLine(challenge.Completing +" "+ challenge.BaseScore * challenge.Ratio +" "+ accepter.Scores);
            if (challenge.Completing != 1 && challenge.BaseScore * challenge.Ratio + previousScore <= accepter.Scores)
            {
                string sql = "UPDATE Challenge SET accepter = " + challengePack.Accepter + 
                    " , completing=1 WHERE challengeId=" + challenge.ChallengeId;
                accepter.Scores -= (int)challenge.BaseScore;       //支付底分
                if (challengeData.UpdateChallenge(sql) && userData.UpdateUser(accepter))
                {
                    for (int i = 0; i < ActiveChallenges.Length; i++)
                    {
                        if (ActiveChallenges[i].ChallengeId == challenge.ChallengeId)
                        {
                            ActiveChallenges[i].Completing = 1;
                            pack.Challengepack[0].ChallengeId = ActiveChallenges[i].ChallengeId;
                            pack.Returncode = ReturnCode.Succeed;
                            break;
                        }
                    }
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                    Console.WriteLine("更新失败"+sql);
                }
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
                Console.WriteLine("挑战已被其他玩家应答或积分不够");
            }
            
            return pack;
        }

        //完成挑战
        public MainPack CompleteChallenge(MainPack pack)
        {
            ChallengePack acceptPack = pack.Challengepack[0];
            
            try
            {
                string sqlupdate = "UPDATE Challenge SET accepterScore = " + acceptPack.AccepterScore
                    + " , finishTime = GETDATE(), completed=1 WHERE challengeId = " + acceptPack.ChallengeId;
                if (challengeData.UpdateChallenge(sqlupdate))
                {
                    for (int i = 0; i < ActiveChallenges.Length; i++)
                    {
                        if (ActiveChallenges[i].ChallengeId == acceptPack.ChallengeId)
                        {
                            Console.WriteLine(ActiveChallenges[i] + "fffff");
                            //ActiveChallenges[i] = null;
                            freeIndex.Enqueue(i);
                            challengeNum[ActiveChallenges[i].Challenger]--;
                            break;
                        }
                    }
                }
                ChallengePack challenge = challengeData.GetCompletedChallenge("SELECT * FROM Challenge WHERE challengeId=" + acceptPack.ChallengeId);
                if (challenge.ChallengerScore > acceptPack.AccepterScore) //结算积分奖励和惩罚,告知玩家结果.
                {
                    ChallengeSettlement(challenge.Challenger, acceptPack.Accepter, false, challenge);
                }
                else if(challenge.ChallengerScore < acceptPack.AccepterScore)
                {
                    ChallengeSettlement(acceptPack.Accepter, challenge.ChallengeId, false, challenge);
                }
                else
                {
                    ChallengeSettlement(challenge.Challenger, acceptPack.Accepter, true, challenge);
                }
                NoticeChallenger(challenge);
                pack.Challengepack.Clear();
                pack.Challengepack.Add(challenge);
                return pack;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return pack;
            }
            
        }

        //结算挑战结果
        public void ChallengeSettlement(int winnerId, int loserId, bool isDraw, ChallengePack challenge)
        {
            if (!isDraw) 
            {
                try
                {
                    double score = challenge.BaseScore * challenge.Ratio;
                    User winner = userData.GetUser(winnerId);
                    User loser = userData.GetUser(loserId);
                    winner.Scores = (int)(winner.Scores + challenge.BaseScore + score);
                    loser.Scores = (int)(loser.Scores - score);
                    userData.UpdateUser(winner);
                    userData.UpdateUser(loser);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message+"--ChallengeSettlement");
                }
            }
            else  //平局 归还底分
            {
                try
                {
                    User player1 = userData.GetUser(winnerId);
                    User player2 = userData.GetUser(loserId);
                    player1.Scores = (int)(player1.Scores + challenge.BaseScore);
                    player2.Scores = (int)(player2.Scores + challenge.BaseScore);
                    userData.UpdateUser(player1);
                    userData.UpdateUser(player2);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "--ChallengeSettlement");
                }
                
            }
        }

        /// <summary>
        /// 将此次挑战存储
        /// </summary>
        /// <param name="challenge"></param>
        public void NoticeChallenger(ChallengePack challenge)
        {
            string sql = "INSERT INTO ChallengeNotice(challengeId,userid,isread) VALUES("
                +challenge.ChallengeId+","+challenge.Challenger+",0)";
            challengeNoticeData.NewNotice(sql);
            
        }

        //挑战发起后超过24小时无人应答,则此次挑战过期作废.
        //每隔20秒对ActiveChallenge[]执行一次检查.
        //public static void ChallengeExpire()
        //{

        //}
    }
}
