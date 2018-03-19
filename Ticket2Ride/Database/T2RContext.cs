using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket2Ride.Database
{
    public class T2RContext : DbContext
    {
        public T2RContext() : base("T2R_test_db")
        {
            
        }

        public DbSet<Connection> Connections { get; set; }
    }
}
