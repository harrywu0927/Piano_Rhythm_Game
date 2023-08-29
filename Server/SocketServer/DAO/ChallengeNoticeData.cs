using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using SocketGameProtocol;
using System.Data;
using System.Data.SqlClient;
using SocketServer.DAO;

namespace SocketServer.DAO
{
    class ChallengeNoticeData
    {
        public bool NewNotice(string sql)
        {
            SqlConnection conn = DBUtil.GetConnection();
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
        public int UpdateNotices(string sql)
        {
            SqlConnection conn = DBUtil.GetConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(sql);
                Console.WriteLine(e.Message);
                return 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();    //关闭数据库连接
                }
            }
        }
        public ChallengePack[] CheckChallengeNotices(string sql)
        {
            SqlConnection conn = DBUtil.GetConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            ArrayList arrayList = new ArrayList();
            ChallengePack[] challenges;
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Google.Protobuf.WellKnownTypes.Timestamp timestamp = new Google.Protobuf.WellKnownTypes.Timestamp();
                        DateTime dateTime = reader.GetDateTime("startTime");
                        TimeSpan elapsedSpan = new TimeSpan(dateTime.Ticks);
                        timestamp.Seconds = (long)elapsedSpan.TotalSeconds;
                        ChallengePack pack = new ChallengePack
                        {
                            Challenger = reader.GetInt32("challenger"),
                            Accepter = reader.GetInt32("accepter"),
                            Ratio = reader.GetDouble("ratio"),
                            BaseScore = reader.GetDouble("base"),
                            Author = reader.GetString("author"),
                            SongName = reader.GetString("songName"),
                            ChallengerScore = reader.GetDouble("challengerScore"),
                            StartTime = timestamp,
                            AccepterScore = reader.GetDouble("accepterScore"),
                            ChallengerName = reader.GetString("challengerName"),
                            ChallengeId = reader.GetInt32("challengeId")
                        };
                        arrayList.Add(pack);
                    }
                    challenges = new ChallengePack[arrayList.Count];
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        challenges[i] = arrayList[i] as ChallengePack;
                    }
                    reader.Close();

                    return challenges;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+sql+ "--GetChallengeNotices");
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
