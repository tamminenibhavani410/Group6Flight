using Group6Flight.Models.DomainModels;

namespace Group6Flight.Models
{
    public interface IFlightRepository : IRepository<Flight>
    {
        bool FlightCodeDateExists(string flightCode, DateTime date);

        IEnumerable<Flight> GetAllFlightsWithAirline();

        IEnumerable<string> GetDistinctFromCities();
        IEnumerable<string> GetDistinctToCities();
        IEnumerable<string> GetCabinTypes();
    }
}