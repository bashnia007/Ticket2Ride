using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Actions;

namespace Ticket2Ride
{
    public class ConnectionBuilder
    {
        public MapConnection MapConnection { get; private set; }
        public ConnectionBuilder(BuildConnectionAction action)
        {
            MapConnection = Table.MapConnections.First(c => c.Connection.Id == action.ObjectId);
        }
        public bool IsValid(BuildConnectionAction buildConnectionAction, Self player)
        {
            var connection = Table.MapConnections.First(c => c.Connection.Id == buildConnectionAction.ObjectId);
            if (!connection.IsFree) return false;
            
            return false;
        }
        
    }
}
