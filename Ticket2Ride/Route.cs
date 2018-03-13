using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket2Ride
{
    public class Route
    {
        public int Id { get; set; }
        public City City1 { get; set; }
        public City City2 { get; set; }
        public int Price { get; set; }
        public bool IsMain { get; set; }
    }
}
