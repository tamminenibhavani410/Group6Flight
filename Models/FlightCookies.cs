namespace Group6Flight.Models
{
    public class FlightCookies
    {
        private const string BookingKey = "myBookings";
        private const string Delimiter = "-";

        private IRequestCookieCollection requestCookies { get; set; } = null!;
        private IResponseCookies responseCookies { get; set; } = null!;

        public FlightCookies(IRequestCookieCollection request, IResponseCookies response)
        {
            requestCookies = request;
            responseCookies = response;
        }
        public void RemoveBookingId(int id)
        {
            string[] ids = GetMyBookingIds();
            var updatedIds = ids.Where(rid => rid != id.ToString()).ToArray();
            SetMyBookingIds(updatedIds);
        }
        public void SetMyBookingIds(List<FlightBooking> myBookings)
        {
            var ids = myBookings.Select(r => r.FlightBookingId.ToString()).ToList();
            SetMyBookingIds(ids);
        }
        public void SetMyBookingIds(IEnumerable<string> ids)
        {
            if (responseCookies == null)
                throw new InvalidOperationException("Response cookies are not initialized.");

            string idsString = string.Join(Delimiter, ids);
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                IsEssential = true
            };

            RemoveMyBookingIds();
            responseCookies.Append(BookingKey, idsString, options);
        }
        public string[] GetMyBookingIds()
        {
            string cookie = requestCookies[BookingKey] ?? String.Empty;
            if (string.IsNullOrEmpty(cookie))
                return Array.Empty<string>();
            else
                return cookie.Split(Delimiter);
        }

        public void RemoveMyBookingIds()
        {
            if (responseCookies == null)
                throw new InvalidOperationException("Response cookies are not initialized.");

            responseCookies.Delete(BookingKey);
        }
    }
}
