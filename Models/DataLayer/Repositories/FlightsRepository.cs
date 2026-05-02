using Group6Flight.Models.DataLayer;
using Group6Flight.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Group6Flight.Models
{
    public class FlightRepository : Repository<Flight>, IFlightRepository
    {
        public FlightRepository(FlightDbContext ctx) : base(ctx) { }

        public bool FlightCodeDateExists(string flightCode, DateTime date)
        {
            return dbset.Any(f =>
                f.FlightCode == flightCode &&
                f.Date.Date == date.Date);
        }

        public IEnumerable<Flight> GetAllFlightsWithAirline()
        {
            return dbset.Include(f => f.Airline).ToList();
        }

        public IEnumerable<string> GetDistinctFromCities()
        {
            return dbset.Select(f => f.From).Distinct().ToList();
        }

        public IEnumerable<string> GetDistinctToCities()
        {
            return dbset.Select(f => f.To).Distinct().ToList();
        }

        public IEnumerable<string> GetCabinTypes()
        {
            return dbset.Select(f => f.CabinType).Distinct().ToList();
        }
    }
}