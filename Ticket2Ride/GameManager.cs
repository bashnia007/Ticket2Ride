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
        public List<MapConnection> MapConnections { get; set; }

        public List<MapCity> MapCities { get; set; }

        public List<Route> Routes { get; set; }
        public Queue<Route> MainRoutes { get; set; }
        public Queue<Route> AuxillaryRoutes { get; set; }

        public List<City> Cities { get; set; }

        public void Init()
        {
            Cards = new List<Card>();
            Deck = new Queue<Card>();
            OpenedCards = new Card[5];

            GetDataFromDb();

            AuxillaryRoutes = new Queue<Route>();

            InitCards();

            MixCards();
            MixRoutes();

            GetInitialOpenCards();

            ProvideInitialSet();
        }
        
        private void ProvideInitialSet()
        {
            ProvideInitialCards();
            ProvideInitialRoutes();

            SetPlayersSettings();
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
                            ProvideRoutes(player);
                            break;
                        case ActionType.BuildCcnnection:
                            BuildConnection(action.ObjectId, action.CardsForPayment, player);
                            break;
                        case ActionType.BuildStation:
                            BuildStation(action.ObjectId, action.CardsForPayment, player);
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

        private void BuildStation(int objectId, List<Card> cardsForPayment, Player player)
        {
            var city = MapCities.Single(c => c.City.Id == objectId);
            if (city.Owner == null)
            {
                city.Owner = player;
                foreach (var card in cardsForPayment)
                {
                    player.Cards.Remove(card);
                }
            }
            else
            {
                throw new ArgumentException("This city is already occupated");
            }
        }

        private void BuildConnection(int objectId, List<Card> cardsForPayment, CardColor color, Player player)
        {
            var connection = MapConnections.Single(c => c.Connection.Id == objectId);
            if (connection.Owner == null)
            {
                if (connection.Connection.IsTunnel)
                {
                    var cardsToAdd = BuildTunnel(color);

                }
                connection.Owner = player;
                foreach (var card in cardsForPayment)
                {
                    player.Cards.Remove(card);
                }
            }
            else
            {
                throw new ArgumentException("This connection is already occupated");
            }
        }

        private int BuildTunnel(CardColor color)
        {
            int additionalCards = 0;
            for (int i = 0; i < Constants.CardsForTunnel; i++)
            {
                if (Deck.Dequeue().Color == color) additionalCards++;
            }
            return additionalCards;
        }

        private void ProvideRoutes(Player player)
        {
            var routes = new List<Route>();
            for (int i = 0; i < Constants.ProvidedRoutes; i++)
            {
                routes.Add(AuxillaryRoutes.Dequeue());
            }
            player.SelectRoutes(routes);
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
            foreach (var player in Players)
            {
                player.Routes.Add(MainRoutes.Dequeue());
                for (int i = 0; i < Constants.InitialAuxillaryRoutesOnHand; i++)
                {
                    player.Routes.Add(AuxillaryRoutes.Dequeue());
                }
            }
        }

        private void SetPlayersSettings()
        {
            foreach (var player in Players)
            {
                player.Stations = Constants.Stations;
                player.Wagons = Constants.WagonsCount;
            }
        }

        private void GetDataFromDb()
        {
            Connections = DbManager.GetConnections();

            Connections.ForEach(connection =>
            {
                MapConnections.Add(new MapConnection(connection));
            });
            var cities = DbManager.GetCities();

            cities.ForEach(city =>
            {
                MapCities.Add(new MapCity(city));
            });
            Routes = DbManager.GetRoutes();
        }
    }
}
