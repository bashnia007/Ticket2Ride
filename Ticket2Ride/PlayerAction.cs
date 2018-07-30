using System;
using System.Collections.Generic;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public class PlayerAction
    {
        public ActionType ActionType { get; set; }
        
        public int ObjectId { get; set; }

        public List<Card> CardsForPayment { get; set; }

        public CardColor CardColor { get; set; }
    }
}
