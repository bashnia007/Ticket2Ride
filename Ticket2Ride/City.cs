using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket2Ride
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Connection> Connections { get; set; }

        public City()
        {
            Connections = new List<Connection>();
        }
    }
}
