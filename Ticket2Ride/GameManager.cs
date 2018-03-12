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
        public void Init()
        {
            Cards = new List<Card>();
            Players = new List<Player>();
            Deck = new Queue<Card>();
            OpenedCards = new Card[5];

            InitCards();
            MixCards();
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
            foreach (Card t in Cards)
            {
                var number = rnd.Next(notMixed.Count);
                var card = notMixed[number];
                Deck.Enqueue(Cards.Single(c => c == card));
            }
        }
    }
}
