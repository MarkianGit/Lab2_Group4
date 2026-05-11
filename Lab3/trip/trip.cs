using System;

namespace Lab3.Models
{
    class Trip
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Budget { get; set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Подорож: {Name}");
            Console.WriteLine($"Країна: {Country}");
            Console.WriteLine($"Дата початку: {StartDate.ToShortDateString()}");
            Console.WriteLine($"Дата завершення: {EndDate.ToShortDateString()}");
            Console.WriteLine($"Бюджет: {Budget} грн");
        }
    }
}