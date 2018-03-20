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
                return db.Connections.ToList();// null; // db.Connections.Include(c => c.Cities).ToList();
            }
        }

        public static void AddFakeConnection()
        {/*
            using (var db = new Database.T2RContext())
            {
                var edinburgh = new City
                {
                    Id = 1,
                    Name = "EDINBURGH"
                };
                db.Cities.Add(edinburgh);

                var london = new City
                {
                    Id = 2,
                    Name = "LONDON"
                };
                db.Cities.Add(london);
                db.SaveChanges();


                var connection1 = new Connection
                {
                    Id = 1,
                    Color = (int) CardColor.Black,
                    Length = 4
                };
                connection1.Cities.Add(edinburgh);
                connection1.Cities.Add(london);
                db.Connections.Add(connection1);

                var connection2 = new Connection
                {
                    Id = 2,
                    Color = (int)CardColor.Orange,
                    Length = 4
                };
                connection2.Cities.Add(edinburgh);
                connection2.Cities.Add(london);
                db.Connections.Add(connection2);
                db.SaveChanges();
            }*/
        }
        
    }
}
