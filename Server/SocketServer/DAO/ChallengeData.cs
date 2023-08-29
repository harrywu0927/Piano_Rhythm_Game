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
    class ChallengeData
    {
        public ChallengePack GetCompletedChallenge(string sql)
        {
            SqlConnection conn = DBUtil.GetConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            ChallengePack challengePack = new ChallengePack();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        challengePack.ChallengeId = reader.GetInt32("challengeId");
                        challengePack.Author = reader.GetString("author");
                        challengePack.Ratio = reader.GetDouble("ratio");
                        challengePack.BaseScore = reader.GetDouble("base");
                        challengePack.SongName = reader.GetString("songName");
                        challengePack.ChallengerScore = reader.GetDouble("challengerScore");
                        challengePack.Completed = reader.GetInt32("completed");
                        challengePack.Completing = reader.GetInt32("completing");
                        //challengePack.StartTime = reader.GetDateTime("startTime");
                        challengePack.Challenger = reader.GetInt32("challenger");
                        challengePack.ChallengerName = reader.GetString("challengerName");
                        challengePack.Accepter = reader.GetInt32("accepter");
                        challengePack.AccepterScore = reader.GetDouble("accepterScore");
                        
                        Console.WriteLine(challengePack);
                    }
                    reader.Close();
                }
                return challengePack;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return challengePack;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();    //关闭数据库连接
                }
            }
        }
        public ChallengePack GetUncompletedChallenge(string sql)
        {
            SqlConnection conn = DBUtil.GetConnection();
            SqlCommand cmd = new SqlCommand(sql, conn);
            ChallengePack challengePack = new ChallengePack();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        challengePack.ChallengeId = reader.GetInt32("challengeId");
                        challengePack.Author = reader.GetString("author");
                        challengePack.Ratio = reader.GetDouble("ratio");
                        challengePack.BaseScore = reader.GetDouble("base");
                        challengePack.SongName = reader.GetString("songName");
                        challengePack.ChallengerScore = reader.GetDouble("challengerScore");
                        challengePack.Completed = reader.GetInt32("completed");
                        challengePack.Completing = reader.GetInt32("completing");
                        //challengePack.StartTime = reader.GetDateTime("startTime");
                        challengePack.Challenger = reader.GetInt32("challenger");
                        challengePack.ChallengerName = reader.GetString("challengerName");

                        //Console.WriteLine(challengePack);
                    }
                    reader.Close();
                }
                return challengePack;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return challengePack;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();    //关闭数据库连接
                }
            }
        }
        public ChallengePack[] GetChallenges(string sql)
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
                        //Google.Protobuf.WellKnownTypes.Timestamp timestamp = new Google.Protobuf.WellKnownTypes.Timestamp();
                        //DateTime dateTime = reader.GetDateTime("completetime");
                        //TimeSpan elapsedSpan = new TimeSpan(dateTime.Ticks);
                        //timestamp.Seconds = (long)elapsedSpan.TotalSeconds;
                        ChallengePack pack = new ChallengePack
                        {
                            Challenger = reader.GetInt32("challenger"),
                            //Accepter = reader.GetInt32("accepter"),
                            Ratio = reader.GetDouble("ratio"),
                            BaseScore = reader.GetDouble("base"),
                            SongName = reader.GetString("songName"),
                            ChallengerScore = reader.GetDouble("challengerScore"),
                            Author = reader.GetString("author")
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
                Console.WriteLine(sql);
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
        public ChallengePack[] GetAllUncompletedChallenges()
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM Challenge WHERE completed = 0";
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
                        //Google.Protobuf.WellKnownTypes.Timestamp timestamp = new Google.Protobuf.WellKnownTypes.Timestamp();
                        //DateTime dateTime = reader.GetDateTime("completetime");
                        //TimeSpan elapsedSpan = new TimeSpan(dateTime.Ticks);
                        //timestamp.Seconds = (long)elapsedSpan.TotalSeconds;
                        ChallengePack pack = new ChallengePack
                        {
                            ChallengeId = reader.GetInt32("challengeId"),
                            Author = reader.GetString("author"),
                            Ratio = reader.GetDouble("ratio"),
                            BaseScore = reader.GetDouble("base"),
                            SongName = reader.GetString("songName"),
                            ChallengerScore = reader.GetDouble("challengerScore"),
                            Completed = reader.GetInt32("completed"),
                            Completing = reader.GetInt32("completing"),
                            //challengePack.StartTime = reader.GetDateTime("startTime");
                            Challenger = reader.GetInt32("challenger"),
                            ChallengerName = reader.GetString("challengerName"),
                    };
                        arrayList.Add(pack);
                        //Console.WriteLine(pack);
                    }
                    challenges = new ChallengePack[arrayList.Count];
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        challenges[i] = arrayList[i] as ChallengePack;
                    }
                    reader.Close();
                    //Console.WriteLine(challenges);
                    return challenges;
                }
                else
                {
                    //Console.WriteLine("null challenge");
                    reader.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+sql+ "--GetAllUncompletedChallenges");
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
        public ChallengePack[] SearchChallengeBySongname(string songname)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM Challenge WHERE songName = '" + songname + "' AND completed = 0";
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
                        DateTime dateTime = reader.GetDateTime("completetime");
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
        public ChallengePack[] SearchChallengeByAuthor(string author)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM Challenge WHERE author = '" + author + "' AND completed = 0";
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
                        DateTime dateTime = reader.GetDateTime("completetime");
                        TimeSpan elapsedSpan = new TimeSpan(dateTime.Ticks);
                        timestamp.Seconds = (long)elapsedSpan.TotalSeconds;
                        ChallengePack pack = new ChallengePack
                        {
                            Challenger = reader.GetInt32("challenger"),
                            Accepter = reader.GetInt32("accepter"),
                            Ratio = reader.GetDouble("ratio"),
                            Author = reader.GetString("author"),
                            BaseScore = reader.GetDouble("base"),
                            SongName = reader.GetString("songName"),
                            ChallengerScore = reader.GetDouble("challengerScore")
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
        public ChallengePack[] SearchChallengeByNameAndAuthor(string author, string songname)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM Challenge WHERE songname='"+songname+"' AND author = '" + author + "' AND completed = 0";
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
                        DateTime dateTime = reader.GetDateTime("completetime");
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
        public ChallengePack[] SearchChallengeByChallenger(int challenger)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM Challenge WHERE challenger = '" + challenger + "' AND completed = 0";
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
                        DateTime dateTime = reader.GetDateTime("completetime");
                        TimeSpan elapsedSpan = new TimeSpan(dateTime.Ticks);
                        timestamp.Seconds = (long)elapsedSpan.TotalSeconds;
                        ChallengePack pack = new ChallengePack
                        {
                            Challenger = reader.GetInt32("challenger"),
                            //Accepter = reader.GetInt32("accepter"),
                            Ratio = reader.GetDouble("ratio"),
                            BaseScore = reader.GetDouble("base"),
                            Author = reader.GetString("author"),
                            SongName = reader.GetString("songName"),
                            ChallengerScore = reader.GetDouble("challengerScore"),
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
                Console.WriteLine(e.Message+ "--SearchChallengeByChallenger");
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
        public int FindChallenge(ChallengePack pack)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql;
            if (pack.ChallengerName != "")
            {
                sql = "SELECT challengeId FROM Challenge WHERE challengerName = '" + pack.ChallengerName + "' AND songName = '"
                + pack.SongName + "' AND author = '" + pack.Author + "' AND completed = 0";
            }
            else
            {
                sql = "SELECT challengeId FROM Challenge WHERE challenger = " + pack.Challenger + " AND songName = '"
                + pack.SongName + "' AND author = '" + pack.Author + "' AND completed = 0";
            }
            //Console.WriteLine(sql);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                int challengeId = -1;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        challengeId = reader.GetInt32("challengeId");
                        //Console.WriteLine(challengeId);
                    }
                    reader.Close();
                }
                //Console.WriteLine(challengeId);
                return challengeId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(sql +"--FindChallenge");
                return -1;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();    //关闭数据库连接
                }
            }
        }
        public bool UpdateChallenge(string sql)
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

        public bool CreateChallenge(ChallengePack pack)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "INSERT INTO Challenge(challenger,ratio,base,challengerScore,startTime," +
                "songName,completed,author,completing,challengerName) VALUES(" + pack.Challenger + "," + pack.Ratio + "," + pack.BaseScore
                + "," + pack.ChallengerScore + ",GETDATE(),'"+pack.SongName+"',0,'"+pack.Author+"',0,'"+pack.ChallengerName+"')";
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
    }
}
