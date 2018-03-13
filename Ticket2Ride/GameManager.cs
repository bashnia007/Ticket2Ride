using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public class GameManager
    {
        public List<Player> Players { get; set; }
        public List<Card> Cards { get; set; }
        public Queue<Card> Deck { get; set; }
        public Card[] OpenedCards { get; set; }

        public List<Connection> Connections { get; set; }

        public List<Route> Routes { get; set; }
        public Queue<Route> MainRoutes { get; set; }
        public Queue<Route> AuxillaryRoutes { get; set; }

        public List<City> Cities { get; set; }

        public void Init()
        {
            Cards = new List<Card>();
            Deck = new Queue<Card>();
            OpenedCards = new Card[5];

            AuxillaryRoutes = new Queue<Route>();

            InitCards();
            MixCards();
            GetInitialOpenCards();
            ProvideInitialCards();

            MixRoutes();
        }

        public void GameProcess()
        {
            if(Players == null) throw new NullReferenceException("Players list is null!");

            bool continueGame = true;
            while (continueGame)
            {
                foreach (var player in Players)
                {
                    var action = player.Action();
                    switch (action.ActionType)
                    {
                        case ActionType.GetCards:
                            ProvideCards(player);
                            break;
                        case ActionType.GetRoutes:
                            ProvideRoutes();
                            break;
                        case ActionType.BuildCcnnection:
                            BuildConnection();
                            break;
                        case ActionType.BuildStation:
                            BuildStation();
                            break;
                        default:
                            break;
                    }
                    if (player.Wagons <= Constants.WagonsForFinish)
                    {
                        continueGame = false;
                        break;
                    }
                }
            }
        }

        private void BuildStation()
        {
            throw new NotImplementedException();
        }

        private void BuildConnection()
        {
            throw new NotImplementedException();
        }

        private void ProvideRoutes()
        {
            throw new NotImplementedException();
        }

        private void ProvideCards(Player player)
        {
            for (int i = 0; i < 2; i++)
            {
                var cardSelection = player.SelectCard(OpenedCards);
                var newCard = Deck.Dequeue();
                if (cardSelection.FromDeck) player.Cards.Add(newCard);
                else
                {
                    player.Cards.Add(new Card
                    {
                        Color = OpenedCards[cardSelection.OpenedCard].Color
                    });
                    OpenedCards[cardSelection.OpenedCard] = newCard;
                }
            }
        }

        private void InitCards()
        {
            for (int color = 0; color < Constants.CardsByColor; color++)
            {
                for (int colorCardId = 0; colorCardId < Constants.CardColorsCount; colorCardId++)
                {
                    Cards.Add(new Card
                    {
                        Color = (CardColor) colorCardId
                    });
                }
            }
            for (int jokerId = 0; jokerId < Constants.Jokers; jokerId++)
            {
                Cards.Add(new Card
                {
                    Color = CardColor.Joker
                });
            }
        }

        private void MixCards()
        {
            Deck = new Queue<Card>();
            var rnd = new Random();
            var notMixed = new List<Card>();
            foreach (var card in Cards)
            {
                notMixed.Add(card);
            }
            foreach (var t in Cards)
            {
                var number = rnd.Next(notMixed.Count);
                var card = notMixed[number];
                Deck.Enqueue(Cards.Single(c => c == card));
                notMixed.Remove(card);
            }
        }

        private void MixRoutes()
        {
            MainRoutes = new Queue<Route>();
            AuxillaryRoutes = new Queue<Route>();
            MixRoutes(Routes.Where(r => r.IsMain).ToList(), MainRoutes);
            MixRoutes(Routes.Where(r => !r.IsMain).ToList(), AuxillaryRoutes);
        }

        private void MixRoutes(List<Route> routes, Queue<Route> result)
        {
            var rnd = new Random();
            var notMixed = new List<Route>();
            foreach (var route in routes)
            {
                notMixed.Add(route);
            }
            foreach (var route in routes)
            {
                var number = rnd.Next(notMixed.Count);
                var selectedRoute = notMixed[number];
                result.Enqueue(Routes.Single(r => r == selectedRoute));
                notMixed.Remove(selectedRoute);
            }
        }

        private void GetInitialOpenCards()
        {
            for (int i = 0; i < Constants.OpenedCards; i++)
            {
                OpenedCards[i] = Deck.Dequeue();
            }
        }

        private void ProvideInitialCards()
        {
            foreach (var player in Players)
            {
                player.Cards = new List<Card>();
                for (int i = 0; i < Constants.InitialCardsOnHand; i++)
                {
                    player.Cards.Add(Deck.Dequeue());
                }
            }
        }

        private void ProvideInitialRoutes()
        {
            
        }
    }
}
