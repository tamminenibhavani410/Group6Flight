using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group6Flight.Models
{
    public class FlightBooking
    {
        public int FlightBookingId { get; set; }
        public int FlightId { get; set; }
        [ValidateNever]
        public Flight Flight { get; set; } = null!;
    }
}
