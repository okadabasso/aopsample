using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Infrastructure
{
    public class TransactionInterceptor: IInterceptor
    {
        public TransactionInterceptor()
        {
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;
            var attributes = method.GetCustomAttributes(typeof(TransactionalAttribute), true);

            if (attributes.Length == 0)
            {
                invocation.Proceed();
                return;
            }
            var service = invocation.InvocationTarget as IService;
            if (service == null)
            {
                invocation.Proceed();
                return;
            }
            System.Data.SqlClient.SqlConnection connection = null;
            try
            {
                connection = service.connection;
                service.transaction  = connection.BeginTransaction();
                invocation.Proceed();

                service.transaction.Commit();
            }
            catch
            {
                if (service.transaction != null)
                {
                    service.transaction.Rollback();
                }
                throw;

            }
            finally
            {
                service.transaction = null;
                connection = null;
                // dispose はできない
            }
        }

    }
}
