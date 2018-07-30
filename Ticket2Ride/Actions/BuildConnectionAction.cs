using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Enums;

namespace Ticket2Ride.Actions
{
    public class BuildConnectionAction : IAction
    {
        public BuildConnectionAction(int connectionId, List<Card> cards)
        {
            ObjectId = connectionId;
            Cards = cards;
        }
        public ActionType ActionType => ActionType.BuildConnection;
        public int ObjectId { get; private set; }
        public List<Card> Cards { get; set; }
    }
}
