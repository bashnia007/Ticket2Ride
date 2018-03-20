using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public class Player
    {
        public Player()
        {
            _rnd = new Random();
            Cards = new List<Card>();
            Routes = new List<Route>();
        }
        public PlayerColor Color { get; set; }
        public int Stations { get; set; }
        public int Wagons { get; set; }
        public List<Card> Cards { get; set; }
        public List<Route> Routes { get; set; }

        private readonly Random _rnd;

        public PlayerAction Action()
        {
            var playerAction = new PlayerAction();
            playerAction.ActionType = ActionType.GetCards;

            return playerAction;
        }

        public CardSelection SelectCard(Card[] openedCards)
        {
            var cardSelection = new CardSelection();
            cardSelection.FromDeck = _rnd.Next(2) == 0;

            if (!cardSelection.FromDeck) cardSelection.OpenedCard = _rnd.Next(Constants.OpenedCards);
            return cardSelection;
        }
    }
}
