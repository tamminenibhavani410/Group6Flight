namespace Group6Flight.Models
{
    public class FlightSessions
    {
        private const string BookingKey = "myBookings";
        private const string CountKey = "bookingsCount";
        private const string ActiveFromKey = "activeFrom";
        private const string ActiveToKey = "activeTo";
        private const string ActiveDepartureDate = "activeDeptDate";
        private const string ActiveCabinType = "activeCabinType";

        private ISession session { get; set; }
        public FlightSessions(ISession session) => this.session = session;

        public void SetMyBookings(List<FlightBooking> flightBookings)
        {
            session.SetObject(BookingKey, flightBookings);
            session.SetInt32(CountKey, flightBookings.Count);
        }
        public List<FlightBooking> GetMyBookings() =>
            session.GetObject<List<FlightBooking>>(BookingKey) ?? new List<FlightBooking>();
        public int? GetMyBookingCount() => session.GetInt32(CountKey);

        public void SetActiveFrom(string activeFrom) =>
            session.SetString(ActiveFromKey, activeFrom);
        public string GetActiveFrom() =>
            session.GetString(ActiveFromKey) ?? string.Empty;

        public void SetActiveTo(string activeTo) =>
            session.SetString(ActiveToKey, activeTo);
        public string GetActiveTo() =>
            session.GetString(ActiveToKey) ?? string.Empty;

        public void SetActiveDepartureDate(string activeDeptDate) =>
            session.SetString(ActiveDepartureDate, activeDeptDate);
        public string GetActiveDepartureDate() =>
            session.GetString(ActiveDepartureDate) ?? string.Empty;

        public void SetActiveCabinType(string activeCabinType) =>
            session.SetString(ActiveCabinType, activeCabinType);
        public string GetActiveCabinType() =>
            session.GetString(ActiveCabinType) ?? string.Empty;
    }
}
