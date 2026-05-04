using Group6Flight.Models;
using Group6Flight.Models.DomainModels;
using Group6Flight.Models.ViewModels;
using Group6Flight.Models.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;

namespace Group6Flight.Controllers
{
    public class HomeController : Controller
    {
        private IFlightRepository flightRepo;
        private IBookingRepository bookingRepo;

        public HomeController(
            IFlightRepository fRepo,
            IBookingRepository bRepo)
        {
            flightRepo = fRepo;
            bookingRepo = bRepo;
        }

        public ViewResult Index(FlightsViewModel model)
        {
            if (model.ActiveDepartureDate == null)
                model.ActiveDepartureDate = "all";

            var session = new FlightSessions(HttpContext.Session);

            session.SetActiveFrom(model.ActiveFromKey);
            session.SetActiveTo(model.ActiveToKey);
            session.SetActiveDepartureDate(model.ActiveDepartureDate);
            session.SetActiveCabinType(model.ActiveCabinType);

            int? count = session.GetMyBookingCount();

            if (!count.HasValue || count == 0)
            {
                var cookies = new FlightCookies(Request.Cookies, Response.Cookies);
                string[] ids = cookies.GetMyBookingIds();

                if (ids.Length > 0)
                {
                    var flights = flightRepo.GetAllFlightsWithAirline()
                        .Where(f => ids.Contains(f.FlightId.ToString()))
                        .ToList();

                    var myBookings = flights.Select(f => new FlightBooking
                    {
                        FlightBookingId = f.FlightId,
                        FlightId = f.FlightId,
                        Flight = f
                    }).ToList();

                    session.SetMyBookings(myBookings);
                }
            }

            var allFlights = flightRepo.GetAllFlightsWithAirline();

            if (!string.IsNullOrEmpty(model.ActiveFromKey) &&
                model.ActiveFromKey.ToLower() != "all")
            {
                allFlights = allFlights
                    .Where(f => f.From == model.ActiveFromKey);
            }

            if (!string.IsNullOrEmpty(model.ActiveToKey) &&
                model.ActiveToKey.ToLower() != "all")
            {
                allFlights = allFlights
                    .Where(f => f.To == model.ActiveToKey);
            }

            if (!string.IsNullOrEmpty(model.ActiveDepartureDate) &&
                model.ActiveDepartureDate.ToLower() != "all")
            {
                DateTime selectedDate = DateTime.Parse(model.ActiveDepartureDate);
                allFlights = allFlights
                    .Where(f => f.Date.Date == selectedDate.Date);
            }

            if (!string.IsNullOrEmpty(model.ActiveCabinType) &&
                model.ActiveCabinType.ToLower() != "all")
            {
                allFlights = allFlights
                    .Where(f => f.CabinType == model.ActiveCabinType);
            }

            model.Flight = allFlights.ToList();

            model.CabinTypes = flightRepo.GetCabinTypes().ToList();
            ViewBag.FromCities = flightRepo.GetDistinctFromCities().ToList();
            ViewBag.ToCities = flightRepo.GetDistinctToCities().ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Booking(int id)
        {
            var session = new FlightSessions(HttpContext.Session);
            var cookies = new FlightCookies(Request.Cookies, Response.Cookies);

            var flight = flightRepo.Get(id);

            if (flight == null)
                return NotFound();

            var bookings = session.GetMyBookings();

            // Check session instead of database
            if (bookings.Any(b => b.FlightId == id))
            {
                TempData["Error"] = "Flight already selected.";
                return RedirectToAction("MyBookings");
            }

            bookings.Add(new FlightBooking
            {
                FlightBookingId = id,
                FlightId = id,
                Flight = flight
            });

            session.SetMyBookings(bookings);
            cookies.SetMyBookingIds(bookings);

            TempData["Message"] = "Flight added successfully!";
            return RedirectToAction("MyBookings");
        }

        [HttpPost]
        public IActionResult BookAllSelected()
        {
            var session = new FlightSessions(HttpContext.Session);
            var cookies = new FlightCookies(Request.Cookies, Response.Cookies);

            var selected = session.GetMyBookings();

            if (selected == null || !selected.Any())
            {
                TempData["Error"] = "No selected flights.";
                return RedirectToAction("MyBookings");
            }

            foreach (var item in selected)
            {
                // check actual flight exists
                var flight = flightRepo.Get(item.FlightId);

                if (flight != null)
                {
                    if (!bookingRepo.IsReserved(item.FlightId))
                    {
                        bookingRepo.Insert(new FlightBooking
                        {
                            FlightId = item.FlightId
                        });
                    }
                }
            }

            bookingRepo.Save();

            session.SetMyBookings(new List<FlightBooking>());
            cookies.RemoveMyBookingIds();

            TempData["Message"] = "All flights reserved successfully!";
            return RedirectToAction("MyBookings");
        }
        public IActionResult MyBookings()
        {
            var session = new FlightSessions(HttpContext.Session);
            var cookies = new FlightCookies(Request.Cookies, Response.Cookies);

            var bookings = session.GetMyBookings();

            if (bookings == null || !bookings.Any())
            {
                var ids = cookies.GetMyBookingIds();

                if (ids.Length > 0)
                {
                    var flights = flightRepo.GetAllFlightsWithAirline()
                        .Where(f => ids.Contains(f.FlightId.ToString()))
                        .ToList();

                    bookings = flights.Select(f => new FlightBooking
                    {
                        FlightBookingId = f.FlightId,
                        FlightId = f.FlightId,
                        Flight = f
                    }).ToList();

                    session.SetMyBookings(bookings);
                }
            }

            var model = new FlightsViewModel
            {
                FlightBooking = bookings,
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CancelBooking(int id)
        {
            var session = new FlightSessions(HttpContext.Session);

            var bookings = session.GetMyBookings();
            var item = bookings.FirstOrDefault(b => b.FlightBookingId == id);

            if (item != null)
            {
                bookings.Remove(item);
                session.SetMyBookings(bookings);
            }

            var cookies = new FlightCookies(Request.Cookies, Response.Cookies);
            cookies.RemoveBookingId(id);

            TempData["Message"] = "Cancelled successfully!";
            return RedirectToAction("MyBookings");
        }

        [HttpPost]
        public IActionResult CancelAllBookings()
        {
            var session = new FlightSessions(HttpContext.Session);

            session.SetMyBookings(new List<FlightBooking>());

            var cookies = new FlightCookies(Request.Cookies, Response.Cookies);
            cookies.RemoveMyBookingIds();

            TempData["Message"] = "All cancelled successfully!";
            return RedirectToAction("MyBookings");
        }

        public IActionResult Details(int id)
        {
            var flight = flightRepo.Get(id);

            if (flight == null)
                return NotFound();

            var session = new FlightSessions(HttpContext.Session);

            var model = new FlightsViewModel
            {
                Flights = flight,
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}