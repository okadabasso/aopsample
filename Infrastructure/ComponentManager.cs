using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.Configuration;
using Castle.MicroKernel.Registration;

namespace Infrastructure
{
    public class ComponentManager
    {
        static IWindsorContainer _container;
        public static IWindsorContainer Container
        {
            get { return _container; }
        }
        public static void Init()
        {
            _container = new WindsorContainer();

            _container.Register(
                Component.For<System.Data.SqlClient.SqlConnection>()
                .UsingFactoryMethod(() => ConnectionFactory.GetConnection())
                .LifestyleScoped()
                ,
                Component.For<TransactionInterceptor>()
                .LifestyleTransient()
                ,
                Types.FromAssemblyNamed("DomainServices")
                    .BasedOn<IService>()
                    .Configure(c => c.Interceptors(typeof(TransactionInterceptor)))
                    .LifestyleTransient()

                                
            );
        }
        public static void Term()
        {
            _container.Dispose();
        }

        public static TComponent GetComponent<TComponent>()
        {
            return _container.Resolve<TComponent>();
        }
    }
}
