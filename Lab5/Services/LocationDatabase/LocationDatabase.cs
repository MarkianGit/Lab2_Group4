using System;
using System.Collections.Generic;
using TravelPlannerApp.Enums;
using TravelPlannerApp.Models;

namespace TravelPlannerApp.Services
{
    class LocationDatabase
    {
        public List<Location> Locations { get; private set; } = new List<Location>();

        public void AddLocation(User user, string locationName)
        {
            if (user.Role == Role.Admin)
            {
                Locations.Add(new Location(locationName));
                Console.WriteLine($"✅ Місто/локацію додано: {locationName}");
            }
            else
            {
                Console.WriteLine("❌ Тільки адмін може додавати нові міста.");
            }
        }

        public void GlobalSearch(User user, string query)
        {
            Console.WriteLine($"\n🔎 Пошук: {query}");

            bool found = false;
            foreach (var loc in Locations)
            {
                if (loc.Name.ToLower().Contains(query.ToLower()))
                {
                    Console.WriteLine("✅ Знайдено: " + loc.Name);
                    found = true;
                }
            }

            if (!found)
                Console.WriteLine("❌ Нічого не знайдено.");
        }
    }
}