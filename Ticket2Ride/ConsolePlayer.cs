using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            return result;
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
    }
}
