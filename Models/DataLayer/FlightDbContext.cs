using Group6Flight.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Group6Flight.Models.DataLayer.Configuration;

namespace Group6Flight.Models.DataLayer
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options)
            : base(options) { }
        public DbSet<Airline> Airline { get; set; } = null!;
        public DbSet<Flight> Flight { get; set; } = null!;
        public DbSet<FlightBooking> FlightBookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigureFlights());
            modelBuilder.ApplyConfiguration(new ConfigureAirlines());
        }
    }
}
