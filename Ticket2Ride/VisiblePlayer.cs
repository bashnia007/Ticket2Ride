using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public class VisiblePlayer
    {
        public PlayerColor PlayerColor { get; set; }
        public int RoutesOnHand { get; set; }
        public int Wagons { get; set; }
        public int Stations { get; set; }
    }
}
