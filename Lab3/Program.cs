using System;
using Lab3.Models;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Trip trip = new Trip();

            trip.Name = "Літня подорож";
            trip.Country = "Італія";
            trip.StartDate = new DateTime(2026, 7, 10);
            trip.EndDate = new DateTime(2026, 7, 20);
            trip.Budget = 50000;

            trip.ShowInfo();

            Expense expense1 = new Expense("Готель", 15000);
            Expense expense2 = new Expense("Квитки", 12000);

            Console.WriteLine();
            Console.WriteLine("Витрати:");
            Console.WriteLine($"{expense1.Category}: {expense1.Amount} грн");
            Console.WriteLine($"{expense2.Category}: {expense2.Amount} грн");

            PackingItem item = new PackingItem("Паспорт");

            item.PackItem();

            Console.WriteLine();
            Console.WriteLine($"Річ: {item.ItemName}");
            Console.WriteLine($"Зібрано: {item.IsPacked}");
        }
    }
}