using System;
using System.Collections.Generic;
using System.Text;
using SocketGameProtocol;
using SocketServer.DAO;

namespace SocketServer.Controller
{
    class GameResultControl
    {
        private GameResultData gameResultData;
        private UserData userData;
        private SongData songData;
        private UsersSongData usersSongData;
        public GameResultControl()
        {
            gameResultData = new GameResultData();
            userData = new UserData();
            songData = new SongData();
            usersSongData = new UsersSongData();
        }

        //游戏结算
        public MainPack GameSettlement(MainPack pack)
        {
            int goldcoins = CalculateGoldcoins(pack.Gameresultpack);
            int experience = CalculateExperience(pack.Gameresultpack);
            pack.Gameresultpack.Goldcoin = goldcoins;
            pack.Gameresultpack.Experience = experience;
            Console.WriteLine("goldcoins:" + goldcoins + " exp:" + experience);
            if (gameResultData.AddGameResult(pack.Gameresultpack))
            {
                pack.Returncode = ReturnCode.Succeed;
                User user = userData.GetUser(pack.Gameresultpack.Userid);
                
                user.Goldcoins += goldcoins;
                user.Experience += experience;
                while (user.Experience >= 1000) //升级
                {
                    user.Level++;
                    user.Experience -= 1000;
                }
                if (userData.UpdateUser(user))//更新用户数据
                {
                    UsersSong[] usersSongs = usersSongData.GetSongsByUserid(user.Userid);
                    pack.Gameresultpack.Isnewrecord = false;
                    for(int i = 0; i < usersSongs.Length; i++) //更新最高分
                    {
                        if (usersSongs[i].Songname == pack.Gameresultpack.Song)
                        {
                            if(usersSongs[i].Scorerecord < pack.Gameresultpack.Gamescore)
                            {
                                usersSongs[i].Scorerecord = pack.Gameresultpack.Gamescore;
                                usersSongData.UpdateUsersSong(usersSongs[i]);
                                pack.Gameresultpack.Isnewrecord = true;
                                break;
                            }
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
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }

        //计算金币奖励
        public int CalculateGoldcoins(GameResultPack result)
        {
            Song song = songData.SerchSongByName(result.Song);
            int difficulty = song.Difficulty;
            int gameScore = (int)(result.Gamescore * 100);
            int combo = result.Combo;
            return (int)(combo * 0.8 * (1 + difficulty * 0.1) + gameScore);
        }

        //计算经验值奖励
        public int CalculateExperience(GameResultPack result)
        {
            Song song = songData.SerchSongByName(result.Song);
            int difficulty = song.Difficulty;
            int perfect = result.Perfect;
            int great = result.Great;
            int good = result.Good;
            return (int)((perfect * 1.2 + great * 1.1 + good) * (1 + difficulty * 0.1));

        }
    }
}
