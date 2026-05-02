using Group6Flight.Models.DomainModels;

namespace Group6Flight.Models
{
    public interface IBookingRepository : IRepository<FlightBooking>
    {
        bool IsReserved(int flightId);
    }
}