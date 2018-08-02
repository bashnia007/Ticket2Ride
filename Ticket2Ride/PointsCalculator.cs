using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket2Ride
{
    public static class PointsCalculator
    {
        private static Dictionary<MapConnection, bool> _connections;
        public static bool IsRouteCompleted(Route route, Player player)
        {
            _isCompleted = false;
            var playerConnections = Table.MapConnections.Where(c => c.Owner == player).ToList();
            _connections = playerConnections.ToDictionary(playerConnection => playerConnection, playerConnection => false);
            
            WidthSearch(route.City1, route.City2);
            return _isCompleted;
        }

        private static bool _isCompleted;
        private static void WidthSearch(City rootCity, City targetCity)
        {
            if (rootCity.Id == targetCity.Id)
            {
                _isCompleted = true;
                return;
            }
            var leaves = _connections
                .Where(c => !c.Value)
                .Where(c => c.Key.Connection.City1.Id == rootCity.Id || c.Key.Connection.City2.Id == rootCity.Id)
                .ToList();

            foreach (var leaf in leaves)
            {
                var newRootCity = leaf.Key.Connection.City1.Id == rootCity.Id ? leaf.Key.Connection.City2 : leaf.Key.Connection.City1;
                _connections[leaf.Key] = true;
                WidthSearch(newRootCity, targetCity);
            }
        }

        public static int CalculatePlayerPoints(Player player)
        {
            int sum = 0;
            sum += SumRoutes(player);
            sum += SumConnections(player);
            sum += SumStations(player);
            sum += LongestRoute(player);
            return sum;
        }

        private static int SumRoutes(Player player)
        {
            int sum = 0;
            foreach (var route in player.Routes)
            {
                if (IsRouteCompleted(route, player))
                {
                    sum += route.Price;
                }
                else
                {
                    sum -= route.Price;
                }
            }
            return sum;
        }

        private static int SumConnections(Player player)
        {
            int sum = 0;
            var playerConnections = Table.MapConnections
                .Where(c => c.Owner == player)
                .Select(c => c.Connection)
                .ToList();

            foreach (var playerConnection in playerConnections)
            {
                switch (playerConnection.Length)
                {
                    case 1:
                        sum += 1;
                        break;
                    case 2:
                        sum += 2;
                        break;
                    case 3:
                        sum += 4;
                        break;
                    case 4:
                        sum += 7;
                        break;
                    case 6:
                        sum += 15;
                        break;
                    case 8:
                        sum += 21;
                        break;
                }
            }
            return sum;
        }

        private static int SumStations(Player player)
        {
            return player.Stations*4;
        }

        private static int LongestRoute(Player player)
        {
            return 0;
        }
    }
}
