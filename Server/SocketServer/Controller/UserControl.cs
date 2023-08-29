using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using SocketGameProtocol;
using SocketServer.DAO;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;

namespace SocketServer.Controller
{
    class UserControl
    {
        private readonly UserData userData;
        private readonly SongData songData;
        private readonly UsersSongData usersSongData;
        private readonly ChallengeNoticeData challengeNoticeData;
        public UserControl()
        {
            userData = new UserData();
            songData = new SongData();
            usersSongData = new UsersSongData();
            challengeNoticeData = new ChallengeNoticeData();
        }

        /// MD5 16位加密
        public static string GetMd5Str(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            //t2 = t2.ToLower();
            return t2;
        }
        // 注册
        public MainPack Signup(MainPack pack)
        {
            pack.Loginpack.Password = GetMd5Str(pack.Loginpack.Password);
            if (userData.Signup(pack)=="Succeed")
            {
                pack.Returncode = ReturnCode.Succeed;
                Console.WriteLine("注册成功");
            }
            else if(userData.Signup(pack) == "User Exists")
            {
                pack.Returncode = ReturnCode.UserExists;
                Console.WriteLine("用户已存在");
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }


        // 登录
        public MainPack Login(MainPack pack)
        {
            pack.Loginpack.Password = GetMd5Str(pack.Loginpack.Password);
            if (userData.Login(pack.Loginpack.Username,pack.Loginpack.Password))
            {
                User user = userData.GetUser(pack.Loginpack.Username);
                pack.User = user;
                //pack.Challengepack.Add(challengeNoticeData.CheckChallengeNotices("SELECT * FROM Challenge,ChallengeNotice  " +
                //    "WHERE Challenge.challengeId = ChallengeNotice.challengeId AND ChallengeNotice.isread = 0" +
                //    " AND ChallengeNotice.userid ="+user.Userid));
                //string sql = "UPDATE ChallengeNotice SET isread = 1 WHERE userid = "+user.Userid;
                //if (pack.Challengepack.Count > 0 && challengeNoticeData.UpdateNotices(sql) == pack.Challengepack.Count)
                //{
                    Console.WriteLine("用户" + user.Usrname + "登录成功");
                    pack.Returncode = ReturnCode.Succeed;
                //}
                
                
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }
        
        //登出
        public MainPack Logout(MainPack pack)
        {
            User user = userData.GetUser(pack.User.Userid);
            user.Online = 0;
            if (userData.UpdateUser(user))
            {
                pack.Returncode = ReturnCode.Succeed;
                Console.WriteLine("用户"+user.Usrname+"登出");
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }

        //购买歌曲
        public MainPack Buysongs(MainPack pack)
        {
            User user = userData.GetUser(pack.User.Userid);
            Song song = songData.SerchSongByName(pack.Searchsongpack.SongName);
            if (!usersSongData.SongExist(pack.Searchsongpack.SongName, pack.User.Userid))
            {
                if (user.Goldcoins >= song.Price)
                {
                    if(user.Level >= song.Requirelevel)
                    {
                        if (userData.CostGoldCoins(user.Userid, song.Price))
                        {
                            usersSongData.AddSong(song.Name, user.Userid);    //添加到用户的歌曲资产
                            song.Downloads++;     //下载量加1
                            songData.UpdateSongData(song);
                            string line = "";   //读取文件并传输
                            using (StreamReader sr = new StreamReader("C:/Users/harry/Desktop/MIDIs/"+song.Name+".txt"))
                            {
                                pack.Songdata.Clear();
                                while ((line = sr.ReadLine()) != null)
                                {
                                    pack.Songdata.Add(line);
                                }
                            }
                            pack.Returncode = ReturnCode.Succeed;
                        }
                        else
                        {
                            pack.Returncode = ReturnCode.Fail;
                        }
                    }
                    else
                    {
                        pack.Returncode = ReturnCode.LevelNotEnough;
                    }
                }
                else
                {
                    pack.Returncode = ReturnCode.GoldcoinNotEnough;
                }
            }
            else
            {
                pack.Returncode = ReturnCode.SongExists;
            }
            
      
            return pack;
        }

        //在已有歌曲中搜索
        public MainPack SearchSongs(MainPack pack)
        {
            pack.Songs.Clear();
            pack.Userssong.Clear();
            if ((pack.Searchsongpack.SongName == "") && (pack.Searchsongpack.Author == ""))
            {
                pack.Authors.Add(songData.GetAllAuthors());
                //Console.WriteLine(pack.User.Userid);
                pack.Userssong.Add(usersSongData.GetAllSongs(pack.User.Userid));
                if (pack.Userssong != null && pack.Authors != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }

            }
            else if ((pack.Searchsongpack.SongName == "") && (pack.Searchsongpack.Author != ""))
            {
                pack.Userssong.Add(usersSongData.GetSongsByAuthor(pack.Searchsongpack.Author));
                if (pack.Userssong != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }
            }
            else if ((pack.Searchsongpack.SongName != "") && (pack.Searchsongpack.Author == ""))
            {
                pack.Userssong.Add(usersSongData.GetSongsByName(pack.Searchsongpack.SongName));
                if (pack.Userssong != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }
            }
            else if ((pack.Searchsongpack.SongName != "") && (pack.Searchsongpack.Author != ""))
            {
                pack.Userssong.Add(usersSongData.GetSongsByNameAndAuthor(pack.Searchsongpack.SongName, pack.Searchsongpack.Author));
                if (pack.Userssong != null)
                {
                    pack.Returncode = ReturnCode.Succeed;
                }
                else
                {
                    pack.Returncode = ReturnCode.Fail;
                }

            }
            return pack;
        }

        public MainPack GetChallengeResult(MainPack pack)
        {
            User user = pack.User;
            ChallengePack[] challengePacks = challengeNoticeData.CheckChallengeNotices("SELECT * FROM Challenge,ChallengeNotice  " +
                "WHERE Challenge.challengeId = ChallengeNotice.challengeId AND ChallengeNotice.isread = 0" +
                " AND ChallengeNotice.userid =" + user.Userid);
            //Console.WriteLine(challengePacks);
            
            string sql = "UPDATE ChallengeNotice SET isread = 1 WHERE userid = " + user.Userid;
            //Console.WriteLine(challengeNoticeData.UpdateNotices(sql)+" "+challengePacks.Length);
            pack.ChallengeNoticePack.Clear();
            if (challengePacks != null)
            {
                ChallengeNotice[] challengeNotices;
                ArrayList arrayList = new ArrayList();
                for (int i=0;i<challengePacks.Length;i++)
                {
                    ChallengeNotice notice = new ChallengeNotice
                    {
                        AccepterScore = challengePacks[i].AccepterScore,
                        AccepterName = userData.GetUser(challengePacks[i].Accepter).Usrname,
                        StartTime = challengePacks[i].StartTime,
                        SongName = challengePacks[i].SongName,
                        ChallengerScore = challengePacks[i].ChallengerScore,
                        Score = challengePacks[i].BaseScore* challengePacks[i].Ratio
                    };
                    if (notice.ChallengerScore > notice.AccepterScore)
                    {
                        notice.IsWin = 1;
                    }
                    else if(notice.ChallengerScore < notice.AccepterScore)
                    {
                        notice.IsWin = 2;
                    }
                    else
                    {
                        notice.IsWin = 3;
                    }
                    //Console.WriteLine(notice);
                    arrayList.Add(notice);
                }
                challengeNotices = new ChallengeNotice[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    challengeNotices[i] = arrayList[i] as ChallengeNotice;
                }
                pack.ChallengeNoticePack.Add(challengeNotices);
                pack.Returncode = ReturnCode.Succeed;
                //Console.WriteLine(pack.ChallengeNoticePack);
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }
    }
}
