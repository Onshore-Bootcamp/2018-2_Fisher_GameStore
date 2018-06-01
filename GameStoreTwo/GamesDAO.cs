using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using GameStoreTwo.Models;
using System.IO;
using System.Data;

namespace GameStoreTwo
{
    public class GamesDAO
    {
        public string currentClass = "GamesDAO";
        private string connString;
        private string filePath;

        public GamesDAO(string dataconnection, string path)
        {
            connString = dataconnection;
            filePath = path;
        }


        //Method that creates a game
        public void CreateNewGame(GamesDO games)
        {
            try
            {
                //Creates a new SQL connection
                using (SqlConnection gameStoreConn = new SqlConnection(connString))
                {
                    //Creating a Command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("CREATE_GAMES", gameStoreConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;

                    //Parameters that are being passed to the stored procedures
                    enterCommand.Parameters.AddWithValue("@GameName", games.GameName);
                    enterCommand.Parameters.AddWithValue("@System", games.System);
                    enterCommand.Parameters.AddWithValue("@Genre", games.Genre);
                    enterCommand.Parameters.AddWithValue("@ReleaseDate", games.ReleaseDate);
                    enterCommand.Parameters.AddWithValue("@Price", games.Price);

                    //Opens the connection
                    gameStoreConn.Open();

                    //Executes Non Query Command
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                GameErrorHandler(false, "fatal", "GameDAO", "CreateNewGame", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        //Creating the items for the System Dropdown list
        public enum System
        {
            //Given Systems
            Xbox_One,
            PlayStation4,
            Xbox_360,
            PlayStation3,
            Nintendo_Switch
        }



        //Method that will display all the games in the database
        public List<GamesDO> ViewAllGames()
        {
            List<GamesDO> allGames = new List<GamesDO>();
            try
            {

                //Creates a SQLConnection
                using (SqlConnection gameStoreConn = new SqlConnection(connString))
                {
                    //Creating a Command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("READ_GAMES", gameStoreConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;

                    //Opens SqlConnection
                    gameStoreConn.Open();

                    //Using a SQLDataAdapter to get the SQL table
                    DataTable gameInfo = new DataTable();
                    using (SqlDataAdapter gameAdapter = new SqlDataAdapter(enterCommand))
                    {
                        gameAdapter.Fill(gameInfo);
                        gameAdapter.Dispose();
                    }

                    //Mapping datarow to a List of the games
                    foreach (DataRow row in gameInfo.Rows)
                    {
                        GamesDO mappedRow = MapAllGames(row);
                        allGames.Add(mappedRow);
                    }
                }


            }
            catch (Exception ex)
            {
                GameErrorHandler(false, "fatal", "GameDAO", "ViewAllGames", ex.Message, ex.StackTrace);
                throw ex;
            }
            //returning a list of the games
            return allGames;
        }

        //Method that views a certain game by it's ID
        public GamesDO ViewGameAtID(Int64 gameId)
        {
            GamesDO games = new GamesDO();
            try
            {
                //Creates a SQLConnection
                using (SqlConnection gameStoreConn = new SqlConnection(connString))
                {
                    //Creating a Command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("VIEW_GAMEID", gameStoreConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;

                    //Parameters that are being passed to the stored procedures
                    enterCommand.Parameters.AddWithValue("@GameID", gameId);

                    //Opens SQLConnection
                    gameStoreConn.Open();

                    //Using a SQLDataAdapter to get the SQL table
                    DataTable gameInfo = new DataTable();
                    using (SqlDataAdapter gameAdapter = new SqlDataAdapter(enterCommand))
                    {
                        gameAdapter.Fill(gameInfo);
                        gameAdapter.Dispose();
                    }

                    //Mapping datarow to a List of the games
                    foreach (DataRow row in gameInfo.Rows)
                    {
                        games = MapAllGames(row);
                    }
                }

            }
            catch (Exception ex)
            {
                GameErrorHandler(false, "fatal", "GameDAO", "ViewGameAtID", ex.Message, ex.StackTrace);

                ex.Data["Message"] = "It Broke";
                throw;
            }
            return games;
        }

        //Method that will Update a games information
        public void UpdateGames(GamesDO games)
        {
            try
            {
                //Creates a SQLConnection
                using (SqlConnection gameStoreConn = new SqlConnection(connString))
                {
                    //Creating a Command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("UPDATE_GAMES", gameStoreConn);

                    //Parameters that are being passed to the stored procedures
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@GameID", games.GameID);
                    enterCommand.Parameters.AddWithValue("@GameName", games.GameName);
                    enterCommand.Parameters.AddWithValue("@System", games.System);
                    enterCommand.Parameters.AddWithValue("@Genre", games.Genre);
                    enterCommand.Parameters.AddWithValue("@ReleaseDate", games.ReleaseDate);
                    enterCommand.Parameters.AddWithValue("@Price", games.Price);

                    //Opens Connection
                    gameStoreConn.Open();

                    //Execute Non Query
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                GameErrorHandler(false, "fatal", "GameDAO", "UpdateGames", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        //Method that will delete a game by it's ID
        public void DeleteGames(Int64 gameId)
        {
            try
            {
                //Creates a SQLConnection to modify the table using a stored procedure for deleting a certain row.
                using (SqlConnection gameStoreConn = new SqlConnection(connString))
                {
                    //Creating a Command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("DELETE_GAMES", gameStoreConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;

                    //Parameters that are being passed to the stored procedures
                    enterCommand.Parameters.AddWithValue("@GameID", gameId);

                    //Opens connection
                    gameStoreConn.Open();

                    //Execute Non Query
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                GameErrorHandler(false, "fatal", "GameDAO", "DeleteGames", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        //Maps all games
        public GamesDO MapAllGames(DataRow dataRow)
        {
            try
            {
                GamesDO games = new GamesDO();

                //If the game ID is not null, then add values to the game object from the database
                if (dataRow["GameID"] != DBNull.Value)
                {
                    games.GameID = (Int64)dataRow["GameID"];
                }
                games.GameName = dataRow["GameName"].ToString();
                games.System = dataRow["System"].ToString();
                games.Genre = dataRow["Genre"].ToString();
                games.ReleaseDate = (DateTime)dataRow["ReleaseDate"];
                games.Price = (decimal)dataRow["Price"];

                //Returning the object with a row updated from SQL
                return games;
            }
            catch (Exception ex)
            {
                GameErrorHandler(false, "fatal", "GameDAO", "MapAllGames", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        //Error Method to write error to a file
        public void GameErrorHandler(bool firstRun, string level, string currentClass, string currentMethod, string message, string stackTrace = null)
        {
            string errorLog = filePath + @"\ErrorLog.txt";
            if (firstRun)
            {
                errorLog = filePath + @"\BackupErrorLog.txt";
            }

            try
            {
                //Using Streamwriter to write error message to a file
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
                //GameErrorHandler(true, "fatal", "GameDAO", "GameErrorHandler", ex.Message, ex.StackTrace);
                throw ex;
            }

        }
    }
}
