using Lab2_Group4.Models;
using Lab2_Group4.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_Group4.ViewModels
{
    public class EventViewModel
    {
        private readonly EventService _service = new();

        public ObservableCollection<EventItem> Events { get; set; } = new();

        public bool IsBusy { get; private set; } = false;
        public bool IsEmpty { get; private set; } = true;

        public string ErrorMessage { get; private set; } = "";

        public string SearchText { get; set; } = "";

        // ====== Auto update filter (№19 базове) ======
        public void Refresh()
        {
            var list = _service.GetAllEvents();

            // регістронезалежний пошук (№8 базове)
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string text = SearchText.Trim().ToLower();
                list = list.Where(x => x.Title.ToLower().Contains(text)).ToList();
            }

            Events.Clear();
            foreach (var item in list)
                Events.Add(item);

            IsEmpty = Events.Count == 0;
        }

        // ====== Add Event Async + IsBusy (№11 базове) ======
        public async Task AddEventAsync(EventItem ev)
        {
            IsBusy = true;

            await Task.Delay(300); // імітація завантаження

            bool ok = _service.AddEvent(ev);
            ErrorMessage = _service.LastError;

            IsBusy = false;

            if (ok)
                Refresh();
        }

        // ====== Delete with confirmation (№6 базове) ======
        public async Task DeleteEventAsync(Guid eventId, string currentUserId, bool confirmed)
        {
            IsBusy = true;
            await Task.Delay(200);

            bool ok = _service.DeleteEvent(eventId, currentUserId, confirmed);
            ErrorMessage = _service.LastError;

            IsBusy = false;

            if (ok)
                Refresh();
        }

        // ====== Geo filter usage ====
        public void FilterByRadius(double userLat, double userLon, double radiusKm)
        {
            var list = _service.SearchByRadius(userLat, userLon, radiusKm);

            Events.Clear();
            foreach (var item in list)
                Events.Add(item);

            IsEmpty = Events.Count == 0;
            ErrorMessage = _service.LastError;
        }

        // ====== Sort by distance ======
        public void SortByDistance(double userLat, double userLon)
        {
            var list = _service.SortByDistance(userLat, userLon);

            Events.Clear();
            foreach (var item in list)
                Events.Add(item);

            IsEmpty = Events.Count == 0;
            ErrorMessage = _service.LastError;
        }

        // ====== Booking ======
        public void AddBookingRequest(Guid eventId, string userId)
        {
            bool ok = _service.AddBookingRequest(eventId, userId);
            ErrorMessage = _service.LastError;
        }

        public void ApproveBooking(Guid requestId, string ownerId)
        {
            bool ok = _service.ApproveBooking(requestId, ownerId);
            ErrorMessage = _service.LastError;
        }

        public string GetOwnerPhone(Guid eventId, string userId)
        {
            string phone = _service.GetOwnerPhone(eventId, userId);
            ErrorMessage = _service.LastError;
            return phone;
        }

        public void CancelBooking(Guid eventId)
        {
            bool ok = _service.CancelBooking(eventId);
            ErrorMessage = _service.LastError;
        }

        // ====== Reviews ======
        public void AddReview(string userId, int rating, string comment)
        {
            bool ok = _service.AddReview(userId, rating, comment);
            ErrorMessage = _service.LastError;
        }

        public double GetTrustRating(string userId)
        {
            return _service.GetUserTrustRating(userId);
        }
    }
}