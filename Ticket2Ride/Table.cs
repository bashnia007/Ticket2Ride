using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket2Ride
{
    public static class Table
    {
        /// <summary>
        /// Список игроков с видимыми параметрами
        /// </summary>
        public static List<VisiblePlayer> VisiblePlayers { get; set; }
        /// <summary>
        /// Список перегонов с указанными владльцами
        /// </summary>
        public static List<MapConnection> MapConnections { get; set; }
        /// <summary>
        /// Список городов с указанными владельцами
        /// </summary>
        public static List<MapCity> MapCities { get; set; }

        public static Card[] OpenedCards { get; set; }
    }
}
