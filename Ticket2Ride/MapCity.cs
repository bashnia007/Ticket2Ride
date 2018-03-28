using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket2Ride
{
    public class MapCity
    {
        public MapCity(City city)
        {
            City = city;
        }
        public City City { get; private set; }
        public Player Owner { get; set; }
    }
}
