using Lab2_Group4.Helpers;
using Lab2_Group4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab2_Group4.Services
{
    public class EventService
    {
        private const int MaxTitleLength = 100;
        private const int MaxDescriptionLength = 1000;

        private readonly List<EventItem> _events = new();
        private readonly List<BookingRequest> _requests = new();
        private readonly Dictionary<string, List<Review>> _reviews = new();

        public string LastError { get; private set; } = "";

        // ====== Safe Execute (базова логіка №15) ======
        private void SafeExecute(Action action)
        {
            try
            {
                LastError = "";
                action();
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
            }
        }

        // ====== Create Event (валідації + trimming + ID) ======
        public bool AddEvent(EventItem ev)
        {
            bool result = false;

            SafeExecute(() =>
            {
                // Trimming (№16)
                ev.Title = ev.Title?.Trim() ?? "";
                ev.Description = ev.Description?.Trim() ?? "";
                ev.OwnerPhone = ev.OwnerPhone?.Trim() ?? "";

                // Обов'язкові поля (№1)
                if (string.IsNullOrWhiteSpace(ev.Title))
                    throw new Exception("Title не може бути порожнім.");

                // Довжина тексту (№7)
                if (ev.Title.Length > MaxTitleLength)
                    throw new Exception($"Title не може бути більше {MaxTitleLength} символів.");

                if (ev.Description.Length > MaxDescriptionLength)
                    throw new Exception($"Description не може бути більше {MaxDescriptionLength} символів.");

                // Логіка дат (№5)
                if (ev.EventDate < DateTime.Now)
                    throw new Exception("Дата події не може бути у минулому.");

                // Валідація телефону Regex (№4)
                if (!Regex.IsMatch(ev.OwnerPhone, @"^\+?[0-9]{10,15}$"))
                    throw new Exception("Невірний формат телефону.");

                // Унікальність ID (№2)
                if (_events.Any(x => x.Id == ev.Id))
                    ev.Id = Guid.NewGuid();

                // Значення за замовчуванням (№12)
                ev.CreatedAt = DateTime.Now;
                ev.IsBooked = false;
                ev.IsDeleted = false;

                _events.Add(ev);
                result = true;
            });

            return result;
        }

        // ====== Soft Delete (№18) ======
        public bool DeleteEvent(Guid eventId, string currentUserId, bool confirmed)
        {
            bool result = false;

            SafeExecute(() =>
            {
                if (!confirmed)
                    throw new Exception("Видалення не підтверджено.");

                var ev = _events.FirstOrDefault(x => x.Id == eventId && !x.IsDeleted);
                if (ev == null)
                    throw new Exception("Подію не знайдено.");

                // Спільний доступ (перевірка власника) (Група4 №8)
                if (ev.OwnerUserId != currentUserId)
                    throw new Exception("Ви не власник події.");

                ev.IsDeleted = true;
                result = true;
            });

            return result;
        }

        // ====== Geo-filter (Група4 №1) ======
        public List<EventItem> SearchByRadius(double userLat, double userLon, double radiusKm)
        {
            List<EventItem> result = new();

            SafeExecute(() =>
            {
                if (radiusKm < 0)
                    throw new Exception("Радіус не може бути від'ємним.");

                result = _events
                    .Where(x => !x.IsDeleted)
                    .Where(x => GeoHelper.GetDistanceKm(userLat, userLon, x.Latitude, x.Longitude) <= radiusKm)
                    .ToList();
            });

            return result;
        }

        // ====== Sort by distance (Група4 №9) ======
        public List<EventItem> SortByDistance(double userLat, double userLon)
        {
            List<EventItem> result = new();

            SafeExecute(() =>
            {
                result = _events
                    .Where(x => !x.IsDeleted)
                    .OrderBy(x => GeoHelper.GetDistanceKm(userLat, userLon, x.Latitude, x.Longitude))
                    .ToList();
            });

            return result;
        }

        // ====== Booking status (Група4 №3) ======
        public bool ApproveBooking(Guid requestId, string ownerId)
        {
            bool result = false;

            SafeExecute(() =>
            {
                var req = _requests.FirstOrDefault(x => x.Id == requestId);
                if (req == null)
                    throw new Exception("Запит не знайдено.");

                var ev = _events.FirstOrDefault(x => x.Id == req.EventId && !x.IsDeleted);
                if (ev == null)
                    throw new Exception("Подію не знайдено.");

                if (ev.OwnerUserId != ownerId)
                    throw new Exception("Ви не власник події.");

                if (ev.IsBooked)
                    throw new Exception("Подія вже заброньована.");

                req.IsApproved = true;
                ev.IsBooked = true;

                result = true;
            });

            return result;
        }

        // ====== FIFO Queue requests (Група4 №5) ======
        public bool AddBookingRequest(Guid eventId, string userId)
        {
            bool result = false;

            SafeExecute(() =>
            {
                var ev = _events.FirstOrDefault(x => x.Id == eventId && !x.IsDeleted);
                if (ev == null)
                    throw new Exception("Подію не знайдено.");

                if (ev.IsBooked)
                    throw new Exception("Подія вже зайнята.");

                // Заборона дублювання запитів
                if (_requests.Any(x => x.EventId == eventId && x.RequestUserId == userId))
                    throw new Exception("Ви вже подали заявку.");

                _requests.Add(new BookingRequest
                {
                    EventId = eventId,
                    RequestUserId = userId,
                    RequestDate = DateTime.Now,
                    IsApproved = false
                });

                // FIFO: автоматично сортуємо чергу
                _requests.Sort((a, b) => a.RequestDate.CompareTo(b.RequestDate));

                result = true;
            });

            return result;
        }

        public List<BookingRequest> GetRequestsForEvent(Guid eventId)
        {
            return _requests
                .Where(x => x.EventId == eventId)
                .OrderBy(x => x.RequestDate)
                .ToList();
        }

        // ====== Privacy phone (Група4 №6) ======
        public string GetOwnerPhone(Guid eventId, string userId)
        {
            string result = "";

            SafeExecute(() =>
            {
                var ev = _events.FirstOrDefault(x => x.Id == eventId && !x.IsDeleted);
                if (ev == null)
                    throw new Exception("Подію не знайдено.");

                bool approved = _requests.Any(x =>
                    x.EventId == eventId &&
                    x.RequestUserId == userId &&
                    x.IsApproved);

                if (!approved)
                    result = "Hidden (Request not approved)";
                else
                    result = ev.OwnerPhone;
            });

            return result;
        }

        // ====== Media validation (Група4 №7) ======
        public bool ValidatePhoto(string fileName, long sizeBytes)
        {
            bool ok = false;

            SafeExecute(() =>
            {
                fileName = fileName?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(fileName))
                    throw new Exception("Файл не вибрано.");

                string ext = System.IO.Path.GetExtension(fileName).ToLower();

                if (ext != ".jpg" && ext != ".png" && ext != ".jpeg")
                    throw new Exception("Дозволені тільки JPG/PNG.");

                long maxSize = 2 * 1024 * 1024; // 2MB
                if (sizeBytes > maxSize)
                    throw new Exception("Файл занадто великий (макс 2MB).");

                ok = true;
            });

            return ok;
        }

        // ====== Cancel restriction (Група4 №10) ======
        public bool CancelBooking(Guid eventId)
        {
            bool result = false;

            SafeExecute(() =>
            {
                var ev = _events.FirstOrDefault(x => x.Id == eventId && !x.IsDeleted);
                if (ev == null)
                    throw new Exception("Подію не знайдено.");

                TimeSpan diff = ev.EventDate - DateTime.Now;

                if (diff.TotalMinutes < 60)
                    throw new Exception("Не можна відмінити бронювання менш ніж за 1 годину.");

                ev.IsBooked = false;

                // також знімаємо approved заявки
                foreach (var req in _requests.Where(x => x.EventId == eventId))
                    req.IsApproved = false;

                result = true;
            });

            return result;
        }

        // ====== Trust rating (Група4 №4) ======
        public double GetUserTrustRating(string userId)
        {
            if (!_reviews.ContainsKey(userId) || _reviews[userId].Count == 0)
                return 0;

            return _reviews[userId].Average(x => x.Rating);
        }

        // ====== Range validation rating 1..5 (№20) ======
        public bool AddReview(string userId, int rating, string comment)
        {
            bool result = false;

            SafeExecute(() =>
            {
                comment = comment?.Trim() ?? "";

                if (rating < 1 || rating > 5)
                    throw new Exception("Рейтинг має бути від 1 до 5.");

                if (!_reviews.ContainsKey(userId))
                    _reviews[userId] = new List<Review>();

                _reviews[userId].Add(new Review
                {
                    UserId = userId,
                    Rating = rating,
                    Comment = comment
                });

                result = true;
            });

            return result;
        }

        // ====== Default sorting newest -> oldest (№9 базове) ======
        public List<EventItem> GetAllEvents()
        {
            return _events
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }
    }
}