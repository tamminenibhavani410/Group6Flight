using Microsoft.EntityFrameworkCore;

namespace Group6Flight.Models
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options)
            : base(options) { }
        public DbSet<Airline> Airline { get; set; } = null!;
        public DbSet<Flight> Flight { get; set; } = null!;
        public DbSet<FlightBooking> FlightBookings{ get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airline>().HasData(
                new Airline { AirlineId = 1, Name = "American Airlines", ImageName = "american_airlines.png" },
                new Airline { AirlineId = 2, Name = "Delta Air Lines", ImageName = "delta_air_lines.png" },
                new Airline { AirlineId = 3, Name = "United Airlines", ImageName = "united_airlines.png" },
                new Airline { AirlineId = 4, Name = "Southwest Airlines", ImageName = "southwest_airlines.png" },
                new Airline { AirlineId = 5, Name = "Alaska Airlines", ImageName = "alaska_airlines.png" },
                new Airline { AirlineId = 6, Name = "JetBlue Airways", ImageName = "jetblue_airways.png" }
            );

            modelBuilder.Entity<Flight>().HasData(
                new Flight
                {
                    FlightId = 1,
                    FlightCode = "AA101",
                    From = "New York",
                    To = "Los Angeles",
                    Date = new DateTime(2026, 4, 10),
                    DepartureTime = new TimeSpan(8, 30, 0),
                    ArrivalTime = new TimeSpan(11, 45, 0),
                    CabinType = "Economy",
                    Emission = 250.5,
                    AircraftType = "Boeing 737",
                    Price = 320.00M,
                    AirlineId = 1,
                },
                new Flight
                {
                    FlightId = 2,
                    FlightCode = "DL202",
                    From = "Atlanta",
                    To = "Chicago",
                    Date = new DateTime(2026, 4, 12),
                    DepartureTime = new TimeSpan(14, 15, 0),
                    ArrivalTime = new TimeSpan(16, 10, 0),
                    CabinType = "Business",
                    Emission = 180.2,
                    AircraftType = "Airbus A320",
                    Price = 450.00M,
                    AirlineId = 2,
                },
                new Flight
                {
                    FlightId = 3,
                    FlightCode = "UA303",
                    From = "San Francisco",
                    To = "Seattle",
                    Date = new DateTime(2026, 4, 15),
                    DepartureTime = new TimeSpan(9, 0, 0),
                    ArrivalTime = new TimeSpan(11, 0, 0),
                    CabinType = "Economy",
                    Emission = 120.0,
                    AircraftType = "Boeing 757",
                    Price = 210.00M,
                    AirlineId = 3,
                },
                new Flight
                {
                    FlightId = 4,
                    FlightCode = "SW404",
                    From = "Dallas",
                    To = "Denver",
                    Date = new DateTime(2026, 4, 18),
                    DepartureTime = new TimeSpan(6, 45, 0),
                    ArrivalTime = new TimeSpan(8, 30, 0),
                    CabinType = "Economy",
                    Emission = 140.7,
                    AircraftType = "Boeing 737",
                    Price = 180.00M,
                    AirlineId = 4,
                },
                new Flight
                {
                    FlightId = 5,
                    FlightCode = "BA505",
                    From = "Boston",
                    To = "London",
                    Date = new DateTime(2026, 4, 20),
                    DepartureTime = new TimeSpan(22, 0, 0),
                    ArrivalTime = new TimeSpan(10, 0, 0), // next day arrival
                    CabinType = "First Class",
                    Emission = 500.0,
                    AircraftType = "Boeing 777",
                    Price = 1200.00M,
                    AirlineId = 5,
                }
            );
        }
    }
}
