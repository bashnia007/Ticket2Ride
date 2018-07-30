using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket2Ride
{
    public static class TableManager
    {
        public static Queue<Card> Deck { get; set; }
        public static Card[] GetCardsForTunnel()
        {
            var result = new Card[3];
            for (int i = 0; i < Constants.CardsForTunnel; i++)
            {
                result[i] = Deck.Dequeue();
            }
            return result;
        }
    }
}
