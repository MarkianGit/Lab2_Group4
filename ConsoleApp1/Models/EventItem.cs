using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab2_Group4.Models
{
    public class EventItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        public DateTime EventDate { get; set; } = DateTime.Now.AddDays(1);

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsBooked { get; set; } = false;

        public string OwnerUserId { get; set; } = "";

        public string OwnerPhone { get; set; } = "";

        public string PhotoFileName { get; set; } = "";

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId { get; set; } = "";
        public int Rating { get; set; } // 1..5
        public string Comment { get; set; } = "";
    }

    public class BookingRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid EventId { get; set; }
        public string RequestUserId { get; set; } = "";

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;
    }
}