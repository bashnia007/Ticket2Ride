using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Actions;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public class GameProcess
    {
        private List<IPlayer> _players; 
        public void Start()
        {
            while (true)
            {
                foreach (var player in _players)
                {
                    var self = GetSelf(player);
                    var action = player.SelectAction(self);
                    switch (action.ActionType)
                    {
                        case ActionType.GetCards:
                            break;
                        case ActionType.BuildConnection:
                            BuildConnection(player, (BuildConnectionAction) action);
                            break;
                        case ActionType.BuildStation:
                            break;
                        case ActionType.GetRoutes:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void BuildConnection(IPlayer player, BuildConnectionAction action)
        {
            var connectionBuilder = new ConnectionBuilder(action);

            if (connectionBuilder.MapConnection.Connection.IsTunnel)
            {
            }
        }

        private Self GetSelf(IPlayer player)
        {
            return new Self();
        }
    }
}
