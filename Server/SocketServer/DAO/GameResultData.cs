using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SocketGameProtocol;
using System.Data;
using System.Data.SqlClient;

namespace SocketServer.DAO
{
    class GameResultData
    {
        public bool AddGameResult(GameResultPack res)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "INSERT INTO GameResult VALUES("+res.Userid+",'"+res.Song+"',"+res.Goldcoin+","+
                res.Experience+","+res.Gamescore+",GETDATE(),"+res.Combo+","+res.Perfect+","+res.Great+
                ","+res.Good+","+res.Miss+")";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (cmd.ExecuteNonQuery() != 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(sql);
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();    //关闭数据库连接
                }
            }
        }
        public GameResultPack[] GetResultsByUserid(int userid)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM GameResult WHERE userid = " + userid;
            SqlCommand cmd = new SqlCommand(sql, conn);
            ArrayList arrayList = new ArrayList();
            GameResultPack[] results;
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {
                        Google.Protobuf.WellKnownTypes.Timestamp timestamp = new Google.Protobuf.WellKnownTypes.Timestamp();
                        DateTime dateTime = reader.GetDateTime("completetime");
                        TimeSpan elapsedSpan = new TimeSpan(dateTime.Ticks);
                        timestamp.Seconds = (long)elapsedSpan.TotalSeconds;
                        GameResultPack res = new GameResultPack
                        {
                            Userid = reader.GetInt32("userid"),
                            Song = reader.GetString("song"),
                            Goldcoin = reader.GetInt32("goldcoins"),
                            Experience = reader.GetInt32("experience"),
                            Gamescore = reader.GetDouble("score"),
                            Completetime = timestamp,
                            Combo = reader.GetInt32("combo"),
                            Perfect = reader.GetInt32("perfect"),
                            Great = reader.GetInt32("great"),
                            Good = reader.GetInt32("good"),
                            Miss = reader.GetInt32("miss")
                        };
                        arrayList.Add(res);
                    }
                    results = new GameResultPack[arrayList.Count];
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        results[i] = arrayList[i] as GameResultPack;
                    }
                    reader.Close();

                    return results;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();    //关闭数据库连接
                }
            }
        }
    }
}
