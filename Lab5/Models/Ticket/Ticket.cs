namespace TravelPlannerApp.Models
{
    class Ticket
    {
        public string Description { get; set; }

        public Ticket(string description)
        {
            Description = description;
        }
    }
}