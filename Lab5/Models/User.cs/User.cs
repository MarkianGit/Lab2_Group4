using TravelPlannerApp.Enums;

namespace TravelPlannerApp.Models
{
    class User
    {
        public string Name { get; set; }
        public Role Role { get; set; }

        public User(string name, Role role)
        {
            Name = name;
            Role = role;
        }
    }
}