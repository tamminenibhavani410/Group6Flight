namespace Group6Flight.Models
{
    public class FlightsViewModel
    {
        public string ActiveFromKey { get; set; } = "all";
        public string ActiveToKey { get; set; } = "all";
        public string ActiveDepartureDate { get; set; } = "all";
        public string ActiveCabinType { get; set; } = "all";
        public List<string> CabinTypes { get; set; } = new List<string>();
        public List<Flight> Flight { get; set; } = new List<Flight>();
        public List<FlightBooking> FlightBooking { get; set; } = new List<FlightBooking>();
        public List<Airline> Airline { get; set; } = new List<Airline>();
        public Flight Flights{ get; set; } = new Flight();
        public FlightBooking FlightBookings{ get; set; } = new FlightBooking();
        public Airline Airlines{ get; set; } = new Airline();

        public string CheckActiveFrom(string d) =>
            d.ToLower() == ActiveFromKey.ToLower() ? "active" : "";
        public string CheckActiveTo(string d) =>
            d.ToLower() == ActiveToKey.ToLower() ? "active" : "";
        public string CheckActiveDepartureDate(string d) =>
            d.ToLower() == ActiveDepartureDate.ToLower() ? "active" : "";
        public string CheckActiveCabinType(string d) =>
            d.ToLower() == ActiveCabinType.ToLower() ? "active" : "";
    }
}
