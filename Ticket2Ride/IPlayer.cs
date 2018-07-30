using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Actions;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public interface IPlayer
    {
        IAction SelectAction(Self self);

        IBuildTunnelAction BuildTunnelAction(int nessesaryCards, CardColor color);
    }
}
