using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Castle.Windsor;
using Castle.MicroKernel.Lifestyle;
namespace Infrastructure
{
    public class ConnectionScope
    {
        public static IDisposable CreateScope()
        {
            return ComponentManager.Container.BeginScope();
        }
    }
}
