using System;
using TravelPlannerApp.Enums;
using TravelPlannerApp.Models;
using TravelPlannerApp.Services;

namespace TravelPlannerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            User organizer = new User("Oleg", Role.Organizer);
            User participant = new User("Ivan", Role.Participant);
            User admin = new User("AdminUser", Role.Admin);

            Trip trip = new Trip("Київ -> Львів");
            trip.Participants.Add(participant);

            LocationDatabase db = new LocationDatabase();
            db.Locations.Add(new Location("Київ"));
            db.Locations.Add(new Location("Львів"));
            db.Locations.Add(new Location("Одеса"));

            Console.WriteLine("=== Travel Planner App ===");

            Console.WriteLine("\n--- Organizer Planner ---");
            trip.EditRoute(organizer, "Київ -> Львів -> Ужгород");
            trip.AddTicket(organizer, "Квиток на потяг Київ-Львів");
            trip.RemoveParticipant(organizer, participant);

            Console.WriteLine("\n--- Participant Info Board ---");
            trip.AddTicket(participant, "Бронь готелю у Львові");
            trip.ShowInfo(participant);

            Console.WriteLine("\n--- Admin Location Database ---");
            db.AddLocation(admin, "Харків");

            Console.WriteLine("\n--- Global Search ---");
            db.GlobalSearch(organizer, "льв");
            db.GlobalSearch(participant, "ха");
            db.GlobalSearch(admin, "ки");

            Console.WriteLine("\n--- Forbidden Actions ---");
            trip.EditRoute(participant, "Не можна змінити");
            db.AddLocation(organizer, "Дніпро");
            trip.AddTicket(admin, "Адмін пробує додати квиток");

            Console.WriteLine("\n=== End ===");
        }
    }
}