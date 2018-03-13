using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public class Connection
    {
        public int Id { get; set; }
        public int JokersRequired { get; set; }
        public int Color { get; set; }
        public City City1 { get; set; }
        public City City2 { get; set; }
        public int Length { get; set; }
    }
}
