using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SocketGameProtocol;
using System.Data;
using System.Data.SqlClient;

namespace SocketServer.DAO
{
    class UsersSongData
    {
        public bool SongExist(string song, int userid)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM UsersSong WHERE songname = '" + song + "' AND userid = " + userid.ToString();
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
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
        public bool AddSong(string name, int userid)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "INSERT INTO UsersSong VALUES('" + name + "'," + userid.ToString() + ",0,0)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (cmd.ExecuteNonQuery() != 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to addsong to user:" + userid.ToString());
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
        public bool UpdateUsersSong(UsersSong userssong)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "UPDATE UsersSong SET scoreRecord=" + userssong.Scorerecord + ", playTimes=playTimes+1" +
                " WHERE songname='" + userssong.Songname + "' AND userid=" + userssong.Userid;
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (cmd.ExecuteNonQuery() != 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(sql);
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
        public UsersSong GetSongByName(string songname)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT UsersSong.userid,UsersSong.songname,UsersSong.scoreRecord,UsersSong.playTimes," +
                "SongData.difficulty,SongData.author FROM SongData, UsersSong WHERE UsersSong.songname = SongData.name" +
                " AND UsersSong.songname ='" + songname + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                UsersSong usersSong = new UsersSong();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        usersSong.Songname = songname;
                        usersSong.Userid = reader.GetInt32("userid");
                        usersSong.Scorerecord = reader.GetDouble("scoreRecord");
                        usersSong.Playtimes = reader.GetInt32("playTimes");
                        usersSong.Difficulty = reader.GetInt32("difficulty");
                        usersSong.Author = reader.GetString("author");
                        usersSong.Rank = GetRank(reader.GetString("Songname"), reader.GetInt32("userid"));
                    }
                    reader.Close();
                }
                return usersSong;
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
        public UsersSong[] GetSongsByAuthor(string author)
        {
            UsersSong[] songs;
            ArrayList arrayList = new ArrayList();
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT UsersSong.userid,UsersSong.songname,UsersSong.scoreRecord,UsersSong.playTimes," +
                "SongData.difficulty,SongData.author FROM SongData, UsersSong WHERE UsersSong.songname = SongData.name" +
                " AND SongData.author='" + author+"'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UsersSong song = new UsersSong
                        {
                            Songname = reader.GetString("songname"),
                            Userid = reader.GetInt32("userid"),
                            Scorerecord = reader.GetDouble("scoreRecord"),
                            Playtimes = reader.GetInt32("playTimes"),
                            Rank = GetRank(reader.GetString("Songname"), reader.GetInt32("userid")),
                            Author = reader.GetString("author"),
                            Difficulty = reader.GetInt32("difficulty")
                        };
                        arrayList.Add(song);
                    }
                    reader.Close();
                }
                songs = new UsersSong[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    songs[i] = arrayList[i] as UsersSong;
                }
                return songs;
            }
            catch (Exception e)
            {
                Console.WriteLine(sql+e.Message);
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
        public UsersSong[] GetSongsByUserid(int userid)
        {
            UsersSong[] songs;
            ArrayList arrayList = new ArrayList();
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT UsersSong.userid,UsersSong.songname,UsersSong.scoreRecord,UsersSong.playTimes," +
                "SongData.difficulty,SongData.author FROM SongData, UsersSong WHERE UsersSong.songname = SongData.name" +
                " AND UsersSong.userid=" + userid;
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UsersSong song = new UsersSong
                        {
                            Songname = reader.GetString("songname"),
                            Userid = reader.GetInt32("userid"),
                            Scorerecord = reader.GetDouble("scoreRecord"),
                            Playtimes = reader.GetInt32("playTimes")
                        };
                        arrayList.Add(song);
                    }
                    reader.Close();
                }
                songs = new UsersSong[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    songs[i] = arrayList[i] as UsersSong;
                }
                return songs;
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
        public UsersSong[] GetAllSongs(int userid)
        {
            UsersSong[] songs;
            ArrayList arrayList = new ArrayList();
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT UsersSong.userid,UsersSong.songname,UsersSong.scoreRecord,UsersSong.playTimes," +
                "SongData.difficulty,SongData.author FROM SongData, UsersSong WHERE UsersSong.songname = SongData.name" +
                " AND UsersSong.userid=" + userid;
            
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UsersSong song = new UsersSong
                        {
                            Songname = reader.GetString("songname"),
                            Userid = reader.GetInt32("userid"),
                            Scorerecord = reader.GetDouble("scoreRecord"),
                            Playtimes = reader.GetInt32("playTimes"),
                            Rank = GetRank(reader.GetString("Songname"),userid),
                            Author = reader.GetString("author"),
                            Difficulty = reader.GetInt32("difficulty")
                        };
                        //Console.WriteLine(song);
                        arrayList.Add(song);
                    }
                    reader.Close();
                }
                songs = new UsersSong[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    songs[i] = arrayList[i] as UsersSong;
                }
                return songs;
            }
            catch (Exception e)
            {
                Console.WriteLine(sql);
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
        public UsersSong[] GetSongsByName(string songname)
        {
            UsersSong[] songs;
            ArrayList arrayList = new ArrayList();
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT UsersSong.userid,UsersSong.songname,UsersSong.scoreRecord,UsersSong.playTimes," +
                "SongData.difficulty,SongData.author FROM SongData, UsersSong WHERE UsersSong.songname = SongData.name" +
                " AND UsersSong.songname LIKE '%" + songname +"%'";

            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UsersSong song = new UsersSong
                        {
                            Songname = reader.GetString("songname"),
                            Userid = reader.GetInt32("userid"),
                            Scorerecord = reader.GetDouble("scoreRecord"),
                            Playtimes = reader.GetInt32("playTimes"),
                            Rank = GetRank(reader.GetString("Songname"), reader.GetInt32("userid")),
                            Author = reader.GetString("author"),
                            Difficulty = reader.GetInt32("difficulty")
                        };
                        arrayList.Add(song);
                    }
                    reader.Close();
                }
                songs = new UsersSong[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    songs[i] = arrayList[i] as UsersSong;
                }
                return songs;
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
        public UsersSong[] GetSongsByNameAndAuthor(string songname, string author)
        {
            UsersSong[] songs;
            ArrayList arrayList = new ArrayList();
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT UsersSong.userid,UsersSong.songname,UsersSong.scoreRecord,UsersSong.playTimes," +
                "SongData.difficulty,SongData.author FROM SongData, UsersSong WHERE UsersSong.songname = SongData.name" +
                " AND UsersSong.songname LIKE '%" + songname + "%' AND SongData.author = '" + author + "'";

            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UsersSong song = new UsersSong
                        {
                            Songname = reader.GetString("songname"),
                            Userid = reader.GetInt32("userid"),
                            Scorerecord = reader.GetDouble("scoreRecord"),
                            Playtimes = reader.GetInt32("playTimes"),
                            Rank = GetRank(reader.GetString("Songname"), reader.GetInt32("userid")),
                            Author = reader.GetString("author"),
                            Difficulty = reader.GetInt32("difficulty")
                        };
                        arrayList.Add(song);
                    }
                    reader.Close();
                }
                songs = new UsersSong[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    songs[i] = arrayList[i] as UsersSong;
                }
                return songs;
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
        public UsersSong GetSongByUseridAndName(int userid, string songname)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT UsersSong.userid,UsersSong.scoreRecord,UsersSong.playTimes," +
                "SongData.difficulty,SongData.author FROM SongData, UsersSong WHERE UsersSong.userid = "+ userid +
                " AND UsersSong.songname ='" + songname + "' AND UsersSong.songname = SongData.name";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                UsersSong usersSong = new UsersSong();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        usersSong.Songname = songname;
                        usersSong.Userid = reader.GetInt32("userid");
                        usersSong.Scorerecord = reader.GetDouble("scoreRecord");
                        usersSong.Playtimes = reader.GetInt32("playTimes");
                        usersSong.Difficulty = reader.GetInt32("difficulty");
                        usersSong.Author = reader.GetString("author");
                        usersSong.Rank = GetRank(songname, reader.GetInt32("userid"));
                    }
                    reader.Close();
                }
                return usersSong;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+ "--GetSongByUseridAndName");
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
        public int GetRank(string songname,int userid)
        {
            int rank = 0;
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT userid FROM UsersSong WHERE songname = '" + songname + "' ORDER BY scoreRecord";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                UsersSong usersSong = new UsersSong();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rank++;
                        if (reader.GetInt32("userid") == userid) break;
                    }
                    reader.Close();
                }
                return rank;
            }
            catch (Exception e)
            {
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
    }
}
