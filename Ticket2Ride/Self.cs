using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public class Self
    {
        public PlayerColor Color { get; set; }
        public int Stations { get; set; }
        public int Wagons { get; set; }
        public List<Card> Cards { get; set; }
        public List<Route> Routes { get; set; }
    }
}
