using GameStoreTwo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreTwo
{
    public class OrdersDAO
    {
        private string connectString;
        private string filePath;

        public OrdersDAO(string dataConnection, string path)
        {
            connectString = dataConnection;
            filePath = path;
        }

        //Method that creates a order
        public void CreateNewOrder(OrdersDO orders, List<int> gameId)
        {
            Int64 uniqueID = 0;
            try
            {
                //Creates a new SQL Connection
                using (SqlConnection orderConn = new SqlConnection(connectString))
                {
                    //Creating a Command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("CREATE_ORDERS", orderConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;

                    //Parameters that are being passed to the stored procedures
                    enterCommand.Parameters.AddWithValue("@Name", orders.Name);
                    enterCommand.Parameters.AddWithValue("@Address", orders.Address);
                    enterCommand.Parameters.AddWithValue("@Phone", orders.Phone);
                    enterCommand.Parameters.AddWithValue("@ZipCode", orders.ZipCode);
                    enterCommand.Parameters.AddWithValue("@UserID", orders.UserID);


                    //Opens SQL connection
                    orderConn.Open();
                    uniqueID = Convert.ToInt64(enterCommand.ExecuteScalar());



                    orderConn.Close();
                    orderConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                OrderErrorHandler(false, "fatal", "OrderDAO", "CreateNewOrder", ex.Message, ex.StackTrace);
                throw ex;
            }

            CreateOrder(gameId, uniqueID);
        }

        public void CreateOrder(List<int> gameId, Int64 orderId)
        {

            try
            {
                using (SqlConnection orderConn = new SqlConnection(connectString))
                {
                    orderConn.Open();
                    foreach (var item in gameId)
                    {
                        SqlCommand enterCommand = new SqlCommand("INSERTINTO_GAMEORDER", orderConn);

                        enterCommand.CommandType = CommandType.StoredProcedure;


                        enterCommand.Parameters.AddWithValue("@GameID", item);
                        enterCommand.Parameters.AddWithValue("@OrderID", orderId);
                        int i = enterCommand.ExecuteNonQuery();
                    }
                    orderConn.Close();



                    orderConn.Dispose();
                }
            }

            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
        }

        //Method that will display all the orders in the database
        public List<OrdersDO> ViewAllOrders()
        {
            List<OrdersDO> allOrders = new List<OrdersDO>();
            try
            {
                //Opens SQL Connection
                using (SqlConnection orderConn = new SqlConnection(connectString))
                {
                    //Creating a Command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("READ_ORDERS", orderConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    orderConn.Open();

                    //Using a SQLDataAdapter to get the SQL table
                    DataTable orderInfo = new DataTable();
                    using (SqlDataAdapter orderAdapter = new SqlDataAdapter(enterCommand))
                    {
                        orderAdapter.Fill(orderInfo);
                        orderAdapter.Dispose();
                    }

                    //Mapping datarow to a List of the orders
                    foreach (DataRow row in orderInfo.Rows)
                    {
                        OrdersDO mappedRows = MapAllOrders(row);
                        allOrders.Add(mappedRows);
                    }

                }

            }
            catch (Exception ex)
            {
                OrderErrorHandler(false, "fatal", "OrderDAO", "ViewAllOrders", ex.Message, ex.StackTrace);

                throw ex;
            }
            //returning a list of the games
            return allOrders;
        }

        //Method that views a certain order by it's ID
        public OrdersDO ViewOrdersByID(Int64 orderId)
        {
            OrdersDO orders = new OrdersDO();

            try
            {
                //Opens SQL Connection


                using (SqlConnection orderConn = new SqlConnection(connectString))
                {
                    //Creating a Command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("VIEW_ORDERID", orderConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("@OrderID", orderId);
                    orderConn.Open();

                    //Using a SQLDataAdapter to get the SQL table
                    DataTable orderInfo = new DataTable();
                    using (SqlDataAdapter orderAdapter = new SqlDataAdapter(enterCommand))
                    {
                        orderAdapter.Fill(orderInfo);
                        orderAdapter.Dispose();
                    }

                    //Maps
                    foreach (DataRow row in orderInfo.Rows)
                    {
                        orders = MapAllOrders(row);
                    }
                }

            }
            catch (Exception ex)
            {
                OrderErrorHandler(false, "fatal", "OrderDAO", "ViewOrdersByID", ex.Message, ex.StackTrace);

                throw;
            }

            return orders;
        }

        public OrdersDO ViewOrderByUser(Int64 orderId)
        {
            OrdersDO orders = new OrdersDO();
            try
            {
                using (SqlConnection orderConn = new SqlConnection(connectString))
                {
                    SqlCommand enterCommand = new SqlCommand("VIEWORDERSBYUSER", orderConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    enterCommand.Parameters.AddWithValue("UserID", orderId);
                    orderConn.Open();

                    DataTable orderInfo = new DataTable();
                    using (SqlDataAdapter orderAdapter = new SqlDataAdapter(enterCommand))
                    {
                        orderAdapter.Fill(orderInfo);
                        orderAdapter.Dispose();
                    }

                    foreach (DataRow row in orderInfo.Rows)
                    {
                        orders = MapAllOrders(row);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return orders;
        }

        //Method that will Update a orders information
        public void UpdateOrder(OrdersDO orders)
        {
            try
            {
                //Opens SQL Connection
                using (SqlConnection orderConn = new SqlConnection(connectString))
                {
                    //Creating a Command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("UPDATE_ORDERS", orderConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;

                    //Parameters that are being passed to the stored procedures
                    enterCommand.Parameters.AddWithValue("@Name", orders.Name);
                    enterCommand.Parameters.AddWithValue("@Address", orders.Address);
                    enterCommand.Parameters.AddWithValue("@Phone", orders.Phone);
                    enterCommand.Parameters.AddWithValue("@ZipCode", orders.ZipCode);
                    enterCommand.Parameters.AddWithValue("@OrderID", orders.OrderID);

                    //Opens Connection
                    orderConn.Open();

                    //Execute Non Query
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                OrderErrorHandler(false, "fatal", "OrderDAO", "UpdateOrder", ex.Message, ex.StackTrace);
                throw;
            }
        }

        //Method that will delete a order by it's ID
        public void DeleteOrder(Int64 orderId)
        {
            try
            {
                //Opens a SQL Connection to modify the table using a stored procedure for deleting a certain row.
                using (SqlConnection orderConn = new SqlConnection(connectString))
                {
                    //Creationg a command to use a stored procedure
                    SqlCommand enterCommand = new SqlCommand("DELETE_ORDERS", orderConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;

                    //parameters that are being passed to the stored procedure
                    enterCommand.Parameters.AddWithValue("@OrderID", orderId);

                    //Open connection
                    orderConn.Open();

                    //Execute Non Query
                    enterCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                OrderErrorHandler(false, "fatal", "OrderDAO", "DeleteOrder", ex.Message, ex.StackTrace);
                throw;
            }
        }

        //Maps all Orders
        public OrdersDO MapAllOrders(DataRow row)
        {
            try
            {
                OrdersDO order = new OrdersDO();

                //If the game ID is not null, then add values to the game object from the database
                if (row["OrderID"] != DBNull.Value)
                {
                    order.OrderID = (Int64)row["OrderID"];
                }
                order.Name = row["Name"].ToString();
                order.Address = row["Address"].ToString();
                order.Phone = row["Phone"].ToString();
                order.ZipCode = row["ZipCode"].ToString();

                //Returning the object with a row updated from SQL
                return order;
            }
            catch (Exception ex)
            {
                OrderErrorHandler(false, "fatal", "OrderDAO", "MapAllOrders", ex.Message, ex.StackTrace);
                throw;
            }
        }

        //Error Method to write error to a file
        public void OrderErrorHandler(bool firstRun, string level, string currentClass, string currentMethod, string message, string stackTrace = null)
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
                OrderErrorHandler(true, "fatal", "GameDAO", "OrderErrorHandler", ex.Message, ex.StackTrace);
                throw ex;
            }

        }
    }
}
