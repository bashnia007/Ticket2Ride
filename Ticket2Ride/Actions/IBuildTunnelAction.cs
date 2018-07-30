using System.Collections.Generic;
using Ticket2Ride.Enums;

namespace Ticket2Ride.Actions
{
    public interface IBuildTunnelAction
    {
        BuildTunnelActionType BuildTunnelActionType { get; set; }
        List<Card> Cards { get; set; }
    }
}
