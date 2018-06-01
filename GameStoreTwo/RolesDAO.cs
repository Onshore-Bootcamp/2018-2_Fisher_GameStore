using GameStoreTwo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GameStoreTwo
{
    public class RolesDAO
    {
        private string roleConnectionString;
        
        public RolesDAO()
        {
            roleConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }
        
        public List<RolesDO> ViewAllRoles()
        {
            List<RolesDO> allRoles = new List<RolesDO>();
            try
            {
                using (SqlConnection roleConn = new SqlConnection(roleConnectionString))
                {
                    SqlCommand enterCommand = new SqlCommand("READ_ROLES", roleConn);
                    enterCommand.CommandType = CommandType.StoredProcedure;
                    roleConn.Open();

                    DataTable roleInfo = new DataTable();
                    using (SqlDataAdapter roleAdapter = new SqlDataAdapter(enterCommand))
                    {
                        roleAdapter.Fill(roleInfo);
                        roleAdapter.Dispose();
                    }

                    foreach(DataRow row in roleInfo.Rows)
                    {
                        RolesDO mappedRow = MapAllRoles(row);
                        allRoles.Add(mappedRow);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return allRoles;
        }

        public RolesDO MapAllRoles(DataRow dataRow)
        {
            RolesDO roles = new RolesDO();
            try
            {


                if (dataRow["RoleID"] != DBNull.Value)
                {
                    roles.RoleID = (Int64)dataRow["RoleID"];
                }
                roles.RoleName = dataRow["Name"].ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
            return roles;
        }
    }
}
