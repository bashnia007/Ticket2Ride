using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket2Ride
{
    public class MapConnection
    {
        public MapConnection(Connection connection)
        {
            Connection = connection;
        }
        public Connection Connection { get; private set; }
        public Player Owner { get; set; }
    }
}
