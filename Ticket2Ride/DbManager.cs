using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public static class DbManager
    {
        public static List<Connection> GetConnections()
        {
            using (var db = new Database.T2RContext())
            {
                //return db.Connections.ToList();// null; 
                return db.Connections.Include(c => c.City1).Include(c => c.City2).ToList();
            }
        }

        public static List<Route> GetRoutes()
        {
            using (var db = new Database.T2RContext())
            {
                return db.Routes.Include(r => r.City1).Include(r => r.City2).ToList();
            }
        }

        public static List<City> GetCities()
        {
            using (var db = new Database.T2RContext())
            {
                return db.Cities.ToList();
            }
        }

        public static void AddFakeConnection()
        {
        }
    }
}
