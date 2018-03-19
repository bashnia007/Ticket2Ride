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
        public int Color { get; set; }
        public int? JokersRequired { get; set; }
        public int Length { get; set; }

        public ICollection<City> Cities { get; set; }

        public Connection()
        {
            Cities = new List<City>();
        }
    }
}
