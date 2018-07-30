using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Actions;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public class FakePlayer : IPlayer
    {
        public IAction SelectAction(Self self)
        {
            return new BuildConnectionAction(1, new List<Card>());
            throw new NotImplementedException();
        }

        public IBuildTunnelAction BuildTunnelAction(int nessesaryCards, CardColor color)
        {
            throw new NotImplementedException();
        }
    }
}
