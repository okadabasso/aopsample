using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ConnectionFactory
    {
        public static System.Data.SqlClient.SqlConnection GetConnection()
        {
            var connection = new System.Data.SqlClient.SqlConnection(
                    System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString
                );
            connection.Open();
            return connection;
        }
    }
}
