using System;
using System.Collections.Generic;

namespace TravelPlannerApp
{
    class Program
    {
        static List<Trip> trips = new List<Trip>();
        static List<Itinerary> itineraries = new List<Itinerary>();
        static List<Ticket> tickets = new List<Ticket>();
        static List<PackingItem> packingItems = new List<PackingItem>();

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== Планувальник подорожей ===");
                Console.WriteLine("1. Додати подорож");
                Console.WriteLine("2. Переглянути подорожі");
                Console.WriteLine("3. Додати активність");
                Console.WriteLine("4. Додати квиток");
                Console.WriteLine("5. Додати річ у список");
                Console.WriteLine("6. Вийти");

                Console.Write("Виберіть пункт: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTrip();
                        break;

                    case "2":
                        ShowTrips();
                        break;

                    case "3":
                        AddItinerary();
                        break;

                    case "4":
                        AddTicket();
                        break;

                    case "5":
                        AddPackingItem();
                        break;

                    case "6":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Невірний вибір!");
                        break;
                }
            }
        }

        static void AddTrip()
        {
            Trip trip = new Trip();

            Console.Write("ID подорожі: ");
            trip.Id = int.Parse(Console.ReadLine());

            Console.Write("Місце призначення: ");
            trip.DestinationName = Console.ReadLine();

            Console.Write("Дата початку: ");
            trip.StartDate = Console.ReadLine();

            Console.Write("Дата завершення: ");
            trip.EndDate = Console.ReadLine();

            Console.Write("Бюджет: ");
            trip.TotalBudget = double.Parse(Console.ReadLine());

            trips.Add(trip);

            Console.WriteLine("Подорож додана!");
        }

        static void ShowTrips()
        {
            Console.WriteLine("\n=== Список подорожей ===");

            foreach (var trip in trips)
            {
                Console.WriteLine($"\nID: {trip.Id}");
                Console.WriteLine($"Місце: {trip.DestinationName}");
                Console.WriteLine($"Початок: {trip.StartDate}");
                Console.WriteLine($"Кінець: {trip.EndDate}");
                Console.WriteLine($"Бюджет: {trip.TotalBudget}");

                Console.WriteLine("\nАктивності:");
                foreach (var item in itineraries)
                {
                    if (item.TripId == trip.Id)
                    {
                        Console.WriteLine($"- {item.ActivityName}");
                    }
                }

                Console.WriteLine("\nКвитки:");
                foreach (var ticket in tickets)
                {
                    if (ticket.TripId == trip.Id)
                    {
                        Console.WriteLine($"- {ticket.Type}");
                    }
                }

                Console.WriteLine("\nСписок речей:");
                foreach (var pack in packingItems)
                {
                    if (pack.TripId == trip.Id)
                    {
                        Console.WriteLine($"- {pack.ItemName}");
                    }
                }

                Console.WriteLine("------------------------");
            }
        }

        static void AddItinerary()
        {
            Itinerary item = new Itinerary();

            Console.Write("ID подорожі: ");
            item.TripId = int.Parse(Console.ReadLine());

            Console.Write("Назва активності: ");
            item.ActivityName = Console.ReadLine();

            Console.Write("Адреса: ");
            item.LocationAddress = Console.ReadLine();

            Console.Write("Час прибуття: ");
            item.ArrivalTime = Console.ReadLine();

            Console.Write("Вартість: ");
            item.Cost = double.Parse(Console.ReadLine());

            itineraries.Add(item);

            Console.WriteLine("Активність додана!");
        }

        static void AddTicket()
        {
            Ticket ticket = new Ticket();

            Console.Write("ID подорожі: ");
            ticket.TripId = int.Parse(Console.ReadLine());

            Console.Write("Тип квитка: ");
            ticket.Type = Console.ReadLine();

            Console.Write("Код бронювання: ");
            ticket.BookingCode = Console.ReadLine();

            tickets.Add(ticket);

            Console.WriteLine("Квиток додано!");
        }

        static void AddPackingItem()
        {
            PackingItem item = new PackingItem();

            Console.Write("ID подорожі: ");
            item.TripId = int.Parse(Console.ReadLine());

            Console.Write("Назва речі: ");
            item.ItemName = Console.ReadLine();

            Console.Write("Запаковано? (true/false): ");
            item.IsPacked = bool.Parse(Console.ReadLine());

            packingItems.Add(item);

            Console.WriteLine("Річ додана!");
        }
    }
}