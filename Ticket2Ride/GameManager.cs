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

        public List<Card> UsedCards { get; set; }

        public List<City> Cities { get; set; }

        public void Init()
        {
            Cards = new List<Card>();
            Deck = new Queue<Card>();
            MapConnections = new List<MapConnection>();
            MapCities = new List<MapCity>();
            OpenedCards = new Card[5];

            GetDataFromDb();

            AuxillaryRoutes = new Queue<Route>();

            InitCards();

            MixCards();
            MixRoutes();

            GetInitialOpenCards();

            ProvideInitialSet();

            FillTable();
        }
        
        private void ProvideInitialSet()
        {
            ProvideInitialCards();
            ProvideInitialRoutes();

            SetPlayersSettings();
        }

        private void FillTable()
        {
            Table.OpenedCards = OpenedCards;
            Table.MapCities = MapCities;
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
                        case ActionType.BuildConnection:
                            BuildConnection(action.ObjectId, action.CardColor, player);
                            break;
                        case ActionType.BuildStation:
                            BuildStation(player);
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

        private void BuildStation(Player player)
        {
            var stationSelection = player.SelectStation();

            UsedCards.AddRange(stationSelection.Cards);
            player.Cards.RemoveAll(c => stationSelection.Cards.Contains(c));

            MapCities.Single(c => c.City.Id == stationSelection.StationId).Owner = player;
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

        private void BuildConnection(int objectId, CardColor color, Player player)
        {
            var connection = MapConnections.Single(c => c.Connection.Id == objectId);
            List<Card> cards;
            if (connection.Connection.IsTunnel)
            {
                int nesessaryAdditionalCards = 0;
                for(int i = 0; i < 3; i++)
                {
                    var card = Deck.Dequeue();
                    if (card.Color == color) nesessaryAdditionalCards++;
                }
                cards = player.BuildTunnel(color, connection.Connection.Length + nesessaryAdditionalCards);
                if (cards != null)
                {
                    Players.Single(p => p.Color == player.Color).Cards.RemoveAll(pc => cards.Select(c => c.Id).ToList().Contains(pc.Id));
                }
                return;
            }
            cards = player.BuildTunnel(color, connection.Connection.Length);
            Players.Single(p => p.Color == player.Color).Cards.RemoveAll(pc => cards.Select(c => c.Id).ToList().Contains(pc.Id));
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
            var selectedRoutes = player.SelectRoutes(1, routes);
            player.Routes.AddRange(selectedRoutes);
        }

        private void ProvideCards(Player player)
        {
            for (int i = 0; i < 2; i++)
            {
                var cardSelection = player.SelectCard();
                var newCard = Deck.Dequeue();
                if (cardSelection.FromDeck) player.Cards.Add(newCard);
                else
                {
                    var selectedCard = OpenedCards[cardSelection.OpenedCard];
                    player.Cards.Add(new Card
                    {
                        Color = selectedCard.Color,
                        Id = selectedCard.Id
                    });
                    OpenedCards[cardSelection.OpenedCard] = newCard;
                    if (selectedCard.Color == CardColor.Joker)
                    {
                        return;
                    }
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
                //todo раскоментировать
                //player.Routes = player.SelectRoutes(1, player.Routes);
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
