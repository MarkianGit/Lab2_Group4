using System;
using System.Collections.Generic;
using TravelPlannerApp.Enums;

namespace TravelPlannerApp.Models
{
    class Trip
    {
        public string Route { get; private set; }
        public List<User> Participants { get; private set; } = new List<User>();
        public List<Ticket> Tickets { get; private set; } = new List<Ticket>();

        public Trip(string route)
        {
            Route = route;
        }

        public void EditRoute(User user, string newRoute)
        {
            if (user.Role == Role.Organizer || user.Role == Role.Admin)
            {
                Route = newRoute;
                Console.WriteLine($"✅ Маршрут змінено на: {Route}");
            }
            else
            {
                Console.WriteLine("❌ У вас немає доступу до редагування маршруту.");
            }
        }

        public void AddTicket(User user, string ticketInfo)
        {
            if (user.Role == Role.Organizer || user.Role == Role.Participant)
            {
                Tickets.Add(new Ticket(ticketInfo));
                Console.WriteLine($"✅ Додано квиток/бронь: {ticketInfo}");
            }
            else
            {
                Console.WriteLine("❌ Адмін не може додавати квитки/броні.");
            }
        }

        public void RemoveParticipant(User user, User participant)
        {
            if (user.Role == Role.Organizer)
            {
                if (Participants.Remove(participant))
                {
                    Console.WriteLine($"✅ Учасника {participant.Name} видалено з поїздки.");
                }
                else
                {
                    Console.WriteLine("❌ Такого учасника немає.");
                }
            }
            else
            {
                Console.WriteLine("❌ Тільки організатор може видаляти учасників.");
            }
        }

        public void ShowInfo(User user)
        {
            if (user.Role == Role.Participant)
            {
                Console.WriteLine("\n📌 Інформаційне табло:");
                Console.WriteLine($"Маршрут: {Route}");

                Console.WriteLine("Квитки/броні:");
                foreach (var t in Tickets)
                    Console.WriteLine("- " + t.Description);
            }
            else
            {
                Console.WriteLine("❌ Це доступно лише учасникам (Participant).");
            }
        }
    }
}