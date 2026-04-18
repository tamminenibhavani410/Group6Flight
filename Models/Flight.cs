using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group6Flight.Models
{
    public class Flight
    {
        public int FlightId { get; set; }

        [Required(ErrorMessage = "Please enter a FlightCode.")]
        [RegularExpression(@"^[A-Za-z]{2}\d{1,4}$",
            ErrorMessage = "FlightCode must be alphanumeric, starting with 2 letters and followed by 1-4 digits.")]
        [Remote(action: "CheckFlightCodeDate", controller: "Validation", areaName: "",
            AdditionalFields = nameof(Date),
            ErrorMessage = "A flight for this date already exists.")]
        public string FlightCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a From.")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed.")]
        public string From { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a To.")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters allowed.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed.")]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a Date.")]
        [FutureDate(3, ErrorMessage = "The date must be valid, later than today, and no more than three years in the future.")]
        [Remote(action: "CheckFlightCodeDate", controller: "Validation", areaName: "",
            AdditionalFields = nameof(FlightCode),
            ErrorMessage = "A flight for this date already exists.")]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage = "Please enter a DepartureTime.")]
        public TimeSpan DepartureTime { get; set; }
        
        [Required(ErrorMessage = "Please enter a ArrivalTime.")]
        public TimeSpan ArrivalTime { get; set; }
        
        [Required(ErrorMessage = "Please enter a CabinType.")]
        public string CabinType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter a Emission.")]
        [Range(0, 5000, ErrorMessage = "CO2 emissions must not exceed 5000 kg CO2e.")]
        public double Emission { get; set; }
        
        [Required(ErrorMessage = "Please enter a AircraftType.")]
        public string AircraftType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter a Price.")]
        [Range(0, 50000, ErrorMessage = "The price must be between 0 and 50,000 USD.")]
        public decimal Price { get; set; }
        public int AirlineId { get; set; }
        [ValidateNever]
        public Airline Airline{ get; set; } = null!;
    }
}
