using Group6Flight.Models.DataLayer;
using Group6Flight.Models.DomainModels;

namespace Group6Flight.Models
{
    public class BookingRepository : Repository<FlightBooking>, IBookingRepository
    {
        public BookingRepository(FlightDbContext ctx) : base(ctx) { }

        public bool IsReserved(int flightId)
        {
            return dbset.Any(r => r.FlightId == flightId);
        }
    }
}