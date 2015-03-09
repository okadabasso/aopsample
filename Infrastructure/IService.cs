using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IService
    {
        System.Data.SqlClient.SqlConnection connection{get; set; }
        System.Data.SqlClient.SqlTransaction transaction{get; set; }

        
    }
}
