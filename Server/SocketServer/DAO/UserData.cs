using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SocketGameProtocol;
using System.Data;
using System.Data.SqlClient;
using SocketServer.DAO;

namespace SocketServer.DAO
{
    class UserData
    {
        public string Signup(MainPack pack)
        {
            string username = pack.Loginpack.Username;
            string password = pack.Loginpack.Password;

            SqlConnection conn = DBUtil.GetConnection();
            try
            {
                string sql = "INSERT INTO Users (username, [password],[level],goldcoins,experience,scores) VALUES ('" + username + "', '" + password + "',0,0,0,0)";
                string sql2 = "SELECT * FROM Users WHERE username='" + username + "'";
                SqlCommand cmd = new SqlCommand(sql2, conn);
                if (cmd.ExecuteReader().HasRows) 
                    return "User Exists";
                SqlCommand cmd2 = new SqlCommand(sql, conn);
                if (cmd2.ExecuteNonQuery()>0)
                {
                    return "Succeed";
                }
                else
                {
                    Console.WriteLine("Failed to insert data into [Users]");
                    return "Database exception";
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to insert data into [Users]\n"+e.Message);
                return "Database exception";
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();    //关闭数据库连接
                }
            }
            

        }
        
        public bool Login(string username, string password)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM Users WHERE username='" + username + "' AND [password]='" + password + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    User user = new User();
                    while (reader.Read())
                    {
                        user.Usrname = reader.GetString("username");
                        user.Userid = reader.GetInt32("userid");
                        user.Level = reader.GetInt32("level");
                        user.Goldcoins = reader.GetInt32("goldcoins");
                        user.Experience = reader.GetInt32("experience");
                        user.Scores = reader.GetInt32("scores");
                        user.Online = reader.GetInt32("online");
                    }
                    //if (user.Online != 0) return false;   
                    reader.Close();
                    string sql2 = "UPDATE Users SET online = 1 WHERE username='" + username + "' AND [password]='" + password + "'";
                    cmd = new SqlCommand(sql2, conn);
                    if (cmd.ExecuteNonQuery() != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    reader.Close();
                    return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Failed to login\n"+e.Message);
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
        public User GetUser(int userid)
        {
            User user;
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM Users WHERE userid=" + userid;
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    user = new User();
                    while (reader.Read())
                    {
                        user.Usrname = reader.GetString("username");
                        user.Userid = reader.GetInt32("userid");
                        user.Level = reader.GetInt32("level");
                        user.Goldcoins = reader.GetInt32("goldcoins");
                        user.Experience = reader.GetInt32("experience");
                        user.Scores = reader.GetInt32("scores");
                        user.Online = reader.GetInt32("online");
                    }
                    reader.Close();
                    return user;
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
        public User GetUser(string username)
        {
            User user;
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "SELECT * FROM Users WHERE username='" + username +"'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    user = new User();
                    while (reader.Read())
                    {
                        user.Usrname = reader.GetString("username");
                        user.Userid = reader.GetInt32("userid");
                        user.Level = reader.GetInt32("level");
                        user.Goldcoins = reader.GetInt32("goldcoins");
                        user.Experience = reader.GetInt32("experience");
                        user.Scores = reader.GetInt32("scores");
                        user.Online = reader.GetInt32("online");
                    }
                    reader.Close();
                    return user;
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
        public bool CostGoldCoins(int userid, int price)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "UPDATE Users SET goldcoins -= " + price.ToString() + " WHERE userid = " + userid.ToString();
            
            SqlCommand cmd = new SqlCommand(sql, conn);
            
            try
            {
                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to cost goldcoins");
                    return false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to cost goldcoins" + e.Message);
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
       
        public bool UpdateUser(User user)
        {
            SqlConnection conn = DBUtil.GetConnection();
            string sql = "UPDATE Users SET goldcoins=" + user.Goldcoins+", [level]= "+user.Level
                        +", experience="+user.Experience+", scores="+user.Scores+", online = "+user.Online+" WHERE userid="+user.Userid;
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (cmd.ExecuteNonQuery()>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(sql+e.Message);
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
