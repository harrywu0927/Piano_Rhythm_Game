using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SocketGameProtocol;
using System.Data;
using System.Data.SqlClient;

namespace SocketServer.DAO
{
    class SongData
    {
        public bool UpdateSongData(Song song)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "UPDATE SongData SET price = "+song.Price +", difficulty="+song.Difficulty+
                ", requirelevel=" + song.Requirelevel+ ", downloads=" + song.Downloads+ ", highestscore=" + song.Highestscore
                +" WHERE name = '"+song.Name+"' AND author='"+song.Author+"'";
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
        public Song[] SerchSongsByName(string name)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM SongData WHERE name LIKE '%" + name + "%'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            Song[] songs;
            ArrayList arrayList = new ArrayList();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {
                        Song song = new Song
                        {
                            Name = reader.GetString("name"),
                            Price = reader.GetInt32("price"),
                            Requirelevel = reader.GetInt32("requirelevel"),
                            Difficulty = reader.GetInt32("difficulty"),
                            Downloads = reader.GetInt32("downloads"),
                            Author = reader.GetString("author")
                        };
                        arrayList.Add(song);
                    }
                    reader.Close();
                    songs = new Song[arrayList.Count];
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        songs[i] = arrayList[i] as Song;
                    }
                    return songs;
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
        public Song SerchSongByName(string name)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM SongData WHERE name = '" + name + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    Song song = new Song();
                    while (reader.Read())
                    {
                        song.Name = reader.GetString("name");
                        song.Price = reader.GetInt32("price");
                        song.Requirelevel = reader.GetInt32("requirelevel");
                        song.Difficulty = reader.GetInt32("difficulty");
                        song.Downloads = reader.GetInt32("downloads");
                        song.Author = reader.GetString("author");
                        
                    }
                    reader.Close();
                    
                    return song;
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
        public Song[] SearchSongsByAuthor(string author)
        {
            Song[] songs;
            ArrayList arrayList = new ArrayList();
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM SongData WHERE author = '" + author + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Song song = new Song
                        {
                            Name = reader.GetString("name"),
                            Price = reader.GetInt32("price"),
                            Requirelevel = reader.GetInt32("requirelevel"),
                            Difficulty = reader.GetInt32("difficulty"),
                            Downloads = reader.GetInt32("downloads"),
                            Author = reader.GetString("author")
                        };
                        arrayList.Add(song);
                    }
                    reader.Close();
                }
                songs = new Song[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    songs[i] = arrayList[i] as Song;
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
        public Song[] SearchSongsByNameAndAuthor(string name, string author)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM SongData WHERE author = '" + author + "' AND name LIKE '%"+name+"%'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            Song[] songs;
            ArrayList arrayList = new ArrayList();
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        Song song = new Song
                        {
                            Name = reader.GetString("name"),
                            Price = reader.GetInt32("price"),
                            Requirelevel = reader.GetInt32("requirelevel"),
                            Difficulty = reader.GetInt32("difficulty"),
                            Downloads = reader.GetInt32("downloads"),
                            Author = reader.GetString("author")
                        };
                        arrayList.Add(song);
                    }
                    reader.Close();
                    songs = new Song[arrayList.Count];
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        songs[i] = arrayList[i] as Song;
                    }
                    return songs;
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
        public Song[] GetAllSongs()
        {
            Song[] songs;
            ArrayList arrayList = new ArrayList();
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM SongData";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Song song = new Song
                        {
                            Name = reader.GetString("name"),
                            Price = reader.GetInt32("price"),
                            Requirelevel = reader.GetInt32("requirelevel"),
                            Difficulty = reader.GetInt32("difficulty"),
                            Downloads = reader.GetInt32("downloads"),
                            Author = reader.GetString("author")
                        };
                        arrayList.Add(song);
                    }

                }
                songs = new Song[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    songs[i] = arrayList[i] as Song;
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
        public Song[] GetAllSongs(int userid)
        {
            Song[] songs;
            ArrayList arrayList = new ArrayList();
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM SongData WHERE name NOT IN (SELECT songname FROM UsersSong WHERE userid = "+userid+")";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Song song = new Song
                        {
                            Name = reader.GetString("name"),
                            Price = reader.GetInt32("price"),
                            Requirelevel = reader.GetInt32("requirelevel"),
                            Difficulty = reader.GetInt32("difficulty"),
                            Downloads = reader.GetInt32("downloads"),
                            Author = reader.GetString("author")
                        };
                        arrayList.Add(song);
                    }

                }
                songs = new Song[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    songs[i] = arrayList[i] as Song;
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
        public string[] GetAllAuthors()
        {
            string[] authors;
            ArrayList arrayList = new ArrayList();
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT DISTINCT author FROM SongData";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string author = reader.GetString("author");
                        arrayList.Add(author);
                    }

                }
                authors = new string[arrayList.Count];
                for (int i = 0; i < arrayList.Count; i++)
                {
                    authors[i] = arrayList[i] as string;
                }
                return authors;
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
