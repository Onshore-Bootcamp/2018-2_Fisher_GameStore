using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStoreTwo.Models;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace GameStoreTwo
{
    public class UserDAO
    {
        private string connectionString;
        private string filePath;

        public UserDAO(string dataConnection, string path)
        {
            connectionString = dataConnection;
            filePath = path;
        }

        public UserDO ReadUserByUsername(string username)
        {
            UserDO user = new UserDO();
            try
            {
                //Opening the SQL Connection
                using (SqlConnection userConn = new SqlConnection(connectionString))
                using (SqlCommand enterCommand = new SqlCommand("READ_USERS", userConn))
                {
                    enterCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@Username", username);

                    userConn.Open();

                    using (SqlDataReader userReader = enterCommand.ExecuteReader())
                    {
                        userReader.Read();
                        user.UserID = userReader.GetInt64(0);
                        user.Firstname = userReader.GetString(1);
                        user.Lastname = userReader.GetString(2);
                        user.Username = userReader.GetString(3);
                        user.Password = userReader.GetString(4);
                        user.EmailAddress = userReader.GetString(5);
                        user.RoleID = userReader.GetInt64(6);
                    }
                }
            }
            catch (Exception ex)
            {
                UserErrorHandler(false, "fatal", "UserDAO", "ReadUserByUsername", ex.Message, ex.StackTrace);
                throw ex;
            }
            return user;
        }

        public UserDO ViewUserAtID(Int64 userID)
        {
            try
            {
                UserDO user = new UserDO();

                using (SqlConnection userConn = new SqlConnection(connectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("VIEW_USERID", userConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@UserID", userID);
                    userConn.Open();

                    DataTable userInfo = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(userInfo);
                        userAdapter.Dispose();
                    }

                    foreach (DataRow row in userInfo.Rows)
                    {
                        user = MapAllUsers(row);
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                UserErrorHandler(false, "fatal", "UserDAO", "ViewUserAtID", ex.Message, ex.StackTrace);

                ex.Data["Message"] = "It Broke";
                throw ex;
            }
        }

        public List<UserDO> ViewAllUsers()
        {
            try
            {
                List<UserDO> allUsers = new List<UserDO>();

                using (SqlConnection userConn = new SqlConnection(connectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("DISPLAY_USERS", userConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    userConn.Open();

                    DataTable userInfo = new DataTable();
                    using (SqlDataAdapter userAdapter = new SqlDataAdapter(enterCommand))
                    {
                        userAdapter.Fill(userInfo);
                        userAdapter.Dispose();
                    }

                    foreach (DataRow row in userInfo.Rows)
                    {
                        UserDO mappedRow = MapAllUsers(row);
                        allUsers.Add(mappedRow);
                    }
                }
                return allUsers;
            }
            catch (Exception ex)
            {
                UserErrorHandler(false, "fatal", "UserDAO", "ViewAllUsers", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public void CreateUser(UserDO user)
        {
            try
            {
                using (SqlConnection userConn = new SqlConnection(connectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("CREATE_USERS", userConn);
                    enterCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    enterCommand.Parameters.AddWithValue("@Firstname", user.Firstname);
                    enterCommand.Parameters.AddWithValue("@Lastname", user.Lastname);
                    enterCommand.Parameters.AddWithValue("@Username", user.Username);
                    enterCommand.Parameters.AddWithValue("@Password", user.Password);
                    enterCommand.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                    enterCommand.Parameters.AddWithValue("@RoleID", user.RoleID);

                    userConn.Open();

                    enterCommand.ExecuteNonQuery();
                    userConn.Close();
                }
            }
            catch (Exception ex)
            {
                UserErrorHandler(false, "fatal", "UserDAO", "CreateUser", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public void UpdateUser(UserDO user)
        {
            try
            {
                using (SqlConnection userConn = new SqlConnection(connectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("UPDATE_USERS", userConn);
                    enterCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    enterCommand.Parameters.AddWithValue("@Firstname", user.Firstname);
                    enterCommand.Parameters.AddWithValue("@Lastname", user.Lastname);
                    enterCommand.Parameters.AddWithValue("@Username", user.Username);
                    enterCommand.Parameters.AddWithValue("@Password", user.Password);
                    enterCommand.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                    enterCommand.Parameters.AddWithValue("@RoleID", user.RoleID);
                    enterCommand.Parameters.AddWithValue("@UserID", user.UserID);

                    userConn.Open();

                    enterCommand.ExecuteNonQuery();
                    userConn.Close();
                }
            }
            catch (Exception ex)
            {
                UserErrorHandler(false, "fatal", "UserDAO", "UpdateUser", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public void DeleteUser(Int64 userId)
        {
            try
            {
                using (SqlConnection userConn = new SqlConnection(connectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("DELETE_USERS", userConn);
                    enterCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    enterCommand.Parameters.AddWithValue("@UserID", userId);

                    userConn.Open();

                    enterCommand.ExecuteNonQuery();
                    userConn.Close();
                }
            }
            catch (Exception ex)
            {
                UserErrorHandler(false, "fatal", "UserDAO", "DeleteUser", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public UserDO MapAllUsers(DataRow dataRow)
        {
            try
            {
                UserDO users = new UserDO();

                if (dataRow["UserID"] != DBNull.Value)
                {
                    users.UserID = (Int64)dataRow["UserID"];
                }
                users.Firstname = dataRow["FirstName"].ToString();
                users.Lastname = dataRow["Lastname"].ToString();
                users.Username = dataRow["Username"].ToString();
                users.Password = dataRow["Password"].ToString();
                users.EmailAddress = dataRow["EmailAddress"].ToString();
                users.RoleID = (Int64)dataRow["RoleID"];

                return users;
            }
            catch (Exception ex)
            {
                UserErrorHandler(false, "fatal", "UserDAO", "MapAllUsers", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public void UserErrorHandler(bool firstRun, string level, string currentClass, string currentMethod, string message, string stackTrace = null)
        {
            string errorLog = filePath + @"\ErrorLog.txt";
            if (firstRun)
            {
                errorLog = filePath + @"\BackupErrorLog.txt";
            }

            try
            {
                using (StreamWriter logWriter = new StreamWriter(errorLog, true))
                {
                    logWriter.WriteLine(new string('-', 120));
                    logWriter.WriteLine($"{DateTime.Now.ToString()} - {level} - {currentClass} - {currentClass}");
                    logWriter.WriteLine(message);
                    if (!string.IsNullOrWhiteSpace(stackTrace))
                    {
                        logWriter.WriteLine(stackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                UserErrorHandler(true, "fatal", "UserDAO", "UserErrorHandler", ex.Message, ex.StackTrace);
                throw ex;
            }

        }
    }
}
