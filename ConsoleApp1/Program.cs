using System;
using System.Threading.Tasks;
using Lab2_Group4.Models;
using Lab2_Group4.ViewModels;

namespace Lab2_Group4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var vm = new EventViewModel();

            Console.WriteLine("=== LAB 2 (Group 4) TEST ===");

            // 1) Створення події
            var ev1 = new EventItem
            {
                Title = "Football match",
                Description = "Friendly match in the park",
                Latitude = 49.798,
                Longitude = 30.115,
                OwnerUserId = "owner1",
                OwnerPhone = "+380931112233",
                EventDate = DateTime.Now.AddDays(2)
            };

            await vm.AddEventAsync(ev1);

            Console.WriteLine("Events count after add: " + vm.Events.Count);

            // 2) Гео-фільтр (радіус 10 км)
            vm.FilterByRadius(49.800, 30.110, 10);
            Console.WriteLine("Events in radius 10 km: " + vm.Events.Count);

            // 3) Сортування за дистанцією
            vm.SortByDistance(49.800, 30.110);
            Console.WriteLine("Sorted by distance (first event title): " + vm.Events[0].Title);

            // 4) Створення заявки на бронювання
            vm.AddBookingRequest(ev1.Id, "user2");
            Console.WriteLine("Booking request created by user2");

            // 5) Власник підтверджує бронювання (але треба отримати requestId)
            // Для тесту зробимо так: додамо ще одну заявку, щоб показати FIFO
            vm.AddBookingRequest(ev1.Id, "user3");
            Console.WriteLine("Booking request created by user3");

            Console.WriteLine("Requests were added (FIFO queue logic inside service).");

            // 6) Додаємо рейтинг власнику (Trust rating)
            vm.AddReview("owner1", 5, "Good organizer!");
            vm.AddReview("owner1", 4, "Everything was fine");

            Console.WriteLine("Owner trust rating: " + vm.GetTrustRating("owner1"));

            // 7) Перевірка приватності телефону
            string phoneHidden = vm.GetOwnerPhone(ev1.Id, "user2");
            Console.WriteLine("Phone for user2 (not approved yet): " + phoneHidden);

            // 8) Видалення події (підтвердження = true)
            await vm.DeleteEventAsync(ev1.Id, "owner1", true);

            Console.WriteLine("Events count after delete: " + vm.Events.Count);

            // 9) Вивід помилки якщо є
            if (!string.IsNullOrWhiteSpace(vm.ErrorMessage))
                Console.WriteLine("ERROR: " + vm.ErrorMessage);

            Console.WriteLine("=== END TEST ===");
            Console.ReadLine();
        }
    }
}