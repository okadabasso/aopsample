using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DomainServices
{
    public class SampleService: Infrastructure.IService
    {
        public SqlConnection connection { get; set; }
        public SqlTransaction transaction { get; set; }


        public SampleService(SqlConnection connection)
        {
            this.connection = connection;
        }

        [Infrastructure.Transactional]
        public virtual bool Insert(string loginName, string password)
        {
            var command = new System.Data.SqlClient.SqlCommand("insert into user_auth(login_name, password) values(@loginName, @password)", connection, transaction);
            command.Parameters.Add(new SqlParameter("@loginName", loginName));
            command.Parameters.Add(new SqlParameter("@password", password));
            
            command.ExecuteNonQuery();
            return true;
        }
        public virtual bool InsertManualTransaction(string loginName, string password)
        {
            try
            {
                transaction = connection.BeginTransaction();

                var command = new System.Data.SqlClient.SqlCommand("insert into user_auth(login_name, password) values(@loginName, @password)", connection, transaction);
                command.CommandText = "insert into user_auth(login_name, password) values(@loginName, @password)";
                command.Parameters.Add(new SqlParameter("@loginName", loginName));
                command.Parameters.Add(new SqlParameter("@password", password));

                command.ExecuteNonQuery();

                transaction.Commit();

            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    transaction = null;
                }

                throw;
            }
            return true;
        }


    }
}
