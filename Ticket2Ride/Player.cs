using System;
using System.Collections.Generic;
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

        public virtual PlayerAction Action()
        {
            var playerAction = new PlayerAction();
            playerAction.ActionType = ActionType.GetCards;

            return playerAction;
        }

        public virtual CardSelection SelectCard()
        {
            var cardSelection = new CardSelection();
            cardSelection.FromDeck = _rnd.Next(2) == 0;

            if (!cardSelection.FromDeck) cardSelection.OpenedCard = _rnd.Next(Constants.OpenedCards);
            return cardSelection;
        }

        public virtual List<Card> BuildTunnel(CardColor cardColor, int cardsCount)
        {
            return new List<Card>();
        }

        public virtual List<Route> SelectRoutes(int min, List<Route> routes)
        {
            throw new NotImplementedException();
        }

        public virtual StationSelection SelectStation()
        {
            throw new NotImplementedException();
        }

        public virtual int SelectConnection(bool isRepeat = false)
        {
            throw new NotImplementedException();
        }

        public virtual CardColor SelectColor()
        {
            throw new NotImplementedException();
        }
    }
}
