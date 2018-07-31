using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket2Ride.Enums;
using Ticket2Ride.Helpers;

namespace Ticket2Ride
{
    public class ConsolePlayer : Player
    {
        public override List<Route> SelectRoutes(int min, List<Route> routes)
        {
            Console.Clear();
            PrintMyself();
            Console.WriteLine($"Выберите не менее {min} маршрутов: ");
            PrintRoutes(routes);
            var input = Console.ReadLine();
            var split = input.Split(' ');
            var result = new List<Route>();
            foreach(var s in split)
            {
                int selected = 0;
                if(!int.TryParse(s, out selected) || selected <=0 || selected > routes.Count)
                {
                    Console.WriteLine("Некорректный ввод");
                    return SelectRoutes(min, routes);
                }
                if(!result.Contains(routes[selected - 1])) result.Add(routes[selected - 1]);
            }
            if(result.Count < min)
            {
                Console.WriteLine("Вы выбрали недостаточное количество маршрутов");
                return SelectRoutes(min, routes);
            }
            return result;
        }
        public override PlayerAction Action()
        {
            PrintMyself();
            PrintTable();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1) Взять карты");
            Console.WriteLine("2) Построить маршрут");
            Console.WriteLine("3) Взять новые маршруты");
            Console.WriteLine("4) Построить станцию");

            var input = Console.ReadLine();
            int selectedAction = 0;
            int.TryParse(input, out selectedAction);

            PlayerAction playerAction = new PlayerAction();
            switch (selectedAction)
            {
                case 1:
                    playerAction.ActionType = Enums.ActionType.GetCards;
                    break;
                case 2:
                    playerAction.ActionType = Enums.ActionType.BuildConnection;
                    break;
                case 3:
                    playerAction.ActionType = Enums.ActionType.GetRoutes;
                    break;
                case 4:
                    playerAction.ActionType = Enums.ActionType.BuildStation;
                    break;
                default:
                    break;
            }
            return playerAction;
        }

        public override CardSelection SelectCard()
        {
            PrintMyself();
            PrintTable();
            var result = new CardSelection();
            Console.WriteLine("Выберите карту. Для выбора из колоды нажмите 0");
            int input = 0;
            if (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Некорректный ввод!");
                return SelectCard();
            }
            result.FromDeck = input == 0;
            result.OpenedCard = input - 1;
            
            return result;
        }

        public override StationSelection SelectStation()
        {
            var result = new StationSelection();
            int stationNumber = Constants.Stations - Stations + 1;
            Console.WriteLine($"Это ваша {stationNumber} станция. Она стоит {stationNumber} карточек строительства");
            Console.WriteLine("Выберите город, где хотите поставить станцию");

            PrintCities();
            int cityId = 0;
            if (!int.TryParse(Console.ReadLine(), out cityId))
            {
                Console.WriteLine("Некорректный ввод!");
                return SelectStation();
            }

            result.StationId = cityId;

            var cardsForPayment = SelectCardsForPayment(stationNumber);
            result.Cards = cardsForPayment;
            return result;
        }

        public override int SelectConnection(bool isRepeat = false)
        {
            if (isRepeat)
            {
                Console.WriteLine("Вы не можете выбрать это соединение!");
            }
            var result = 0;
            Console.WriteLine("Выберите соединение");
            PrintConnections();

            if (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Некорректный ввод!");
                return SelectConnection();
            }

            return result;
        }

        public override CardColor SelectColor()
        {
            Console.WriteLine("Выберите цвет");
            foreach (var value in Enum.GetNames(typeof(CardColor)))
            {
                Console.WriteLine(value);
            }
            var result = Console.ReadLine();
            return CardColor.Black;
        }

        private void PrintTable()
        {
            Console.WriteLine("Карты на столе: ");
            foreach(var card in Table.OpenedCards)
            {
                Console.Write(card.Color + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private void PrintMyself()
        {
            Console.Clear();
            Console.WriteLine("Цвет: " + Color);
            Console.WriteLine("Станций: " + Stations);
            Console.WriteLine("Вагонов: " + Wagons);
            Console.WriteLine("Карты строительства: ");
            foreach(var card in Cards)
            {
                Console.Write(card.Color + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Карты маршрутов: ");
            PrintRoutes(Routes);
            Console.WriteLine();
        }

        private void PrintRoutes(List<Route> routes)
        {
            foreach (var route in routes)
            {
                Console.WriteLine($"{routes.IndexOf(route) + 1}) {route.City1.Name} - {route.City2.Name}, {route.Price}");
            }
        }

        private void PrintCities()
        {
            foreach (var mapCity in Table.MapCities)
            {
                var state = mapCity.Owner?.Color.ToString() ?? "Свободен";
                Console.WriteLine($"{mapCity.City.Id} {mapCity.City.Name} {state}");
            }
        }

        private void PrintConnections()
        {
            foreach (var connection in Table.MapConnections)
            {
                var isTunnel = connection.Connection.IsTunnel ? "Тунель," : "";
                var isFree = connection.IsFree ? "Свободен" : $"владелец: {connection.Owner.Color}, ";
                var jokers = (connection.Connection.JokersRequired != null && connection.Connection.JokersRequired > 0) ? $"требуется {connection.Connection.JokersRequired} паровозов" : "";
                Console.WriteLine($"{connection.Connection.Id}: " +
                                  $"{connection.Connection.City1.Name} - " +
                                  $"{connection.Connection.City2.Name}, " +
                                  $"цвет={DescriptionHelper.GetEnumDescription((CardColor)connection.Connection.Color)}, " +
                                  $"длина={connection.Connection.Length}, " +
                                  $"{isTunnel} " +
                                  $"{isFree} " +
                                  $"{jokers}");
            }
        }

        private List<Card> SelectCardsForPayment(int count)
        {
            var result = new List<Card>();
            Console.WriteLine("Выберите карты строительства:");
            foreach (var card in Cards)
            {
                Console.WriteLine($"{Cards.IndexOf(card) + 1} {card.Color}");
            }

            for (int i = 0; i < count; i++)
            {
                int selected = 0;
                if (!int.TryParse(Console.ReadLine(), out selected))
                {
                    Console.WriteLine("Некорректный ввод!");
                    i--;
                    continue;
                }
                var card = Cards[selected - 1];
                if (result.Count > 0 && card.Color != result[0].Color)
                {
                    Console.WriteLine("Нельзя взять эту карту!");
                    i--;
                    continue;
                }
                result.Add(card);
            }

            return result;
        }
    }
}
