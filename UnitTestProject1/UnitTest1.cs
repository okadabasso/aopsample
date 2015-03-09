using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.Configuration;
using Castle.MicroKernel.Registration;


namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            Infrastructure.ComponentManager.Init();


        }
        [ClassCleanup]
        public static void CleanupClass()
        {
            Infrastructure.ComponentManager.Term();
        }
        [TestInitialize]
        public void TearUp()
        {
            var connection = Infrastructure.ConnectionFactory.GetConnection();
            var command = new System.Data.SqlClient.SqlCommand ("truncate table user_auth");
            command.Connection = connection;
            command.ExecuteNonQuery();
            connection.Close();
        }
        [TestMethod]
        public void TestMethod1()
        {
            using (Infrastructure.ConnectionScope.CreateScope())
            {
                var service = Infrastructure.ComponentManager.GetComponent<DomainServices.SampleService>();
                var result = service.Insert("login 1","password 1");
                result = service.Insert("login 2", "password 2");
            }

            Assert.AreEqual(2, getCount());
        }
        [TestMethod]
        public void TestManualTransaction()
        {
            using (Infrastructure.ConnectionScope.CreateScope())
            {
                var service = Infrastructure.ComponentManager.GetComponent<DomainServices.SampleService>();
                var result = service.InsertManualTransaction("login 1", "password 1");
                result = service.InsertManualTransaction("login 2", "password 2");
            }

            Assert.AreEqual(2, getCount());
        }

        public int getCount()
        {
            var connection = Infrastructure.ConnectionFactory.GetConnection();
            var command = new System.Data.SqlClient.SqlCommand("select count(*)from user_auth");
            command.Connection = connection;
            int count = (int)command.ExecuteScalar();

            connection.Close();
            return count;

        }
    }
}
