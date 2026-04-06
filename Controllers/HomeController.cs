using Group6Flight.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group6Flight.Controllers
{
    public class HomeController : Controller
    {
        private FlightDbContext _context;
        public HomeController(FlightDbContext context)
        {
            _context = context;
        }

        public ViewResult Index(FlightsViewModel model)
        {
            var DeptDate = model.ActiveDepartureDate;
            if (DeptDate == null)
            {
                model.ActiveDepartureDate = "all";
            }
            var session = new FlightSessions(HttpContext.Session);
            session.SetActiveFrom(model.ActiveFromKey);
            session.SetActiveTo(model.ActiveToKey);
            session.SetActiveDepartureDate(model.ActiveDepartureDate);
            session.SetActiveCabinType(model.ActiveCabinType);
            
            int? count = session.GetMyBookingCount();
            if (!count.HasValue)
            {
                var cookies = new FlightCookies(Request.Cookies, Response.Cookies);
                string[] ids = cookies.GetMyBookingIds();

                if (ids.Length > 0)
                {
                    var myBookings = _context.FlightBookings
                        .Include(r => r.Flight)
                        .Where(r => ids.Contains(r.FlightBookingId.ToString()))
                        .ToList();

                    session.SetMyBookings(myBookings);
                }
            }

            IQueryable<Flight> query = _context.Flight
                .Include(r => r.Airline)
                .OrderBy(r => r.FlightCode);

            if (!string.IsNullOrEmpty(model.ActiveFromKey) && model.ActiveFromKey.ToLower() != "all")
            {
                query = query.Where(r => r.From.ToString() == model.ActiveFromKey);
            }

            if (!string.IsNullOrEmpty(model.ActiveToKey) && model.ActiveToKey.ToLower() != "all")
            {
                query = query.Where(r => r.To == model.ActiveToKey);
            }
            if (!string.IsNullOrEmpty(model.ActiveDepartureDate) && model.ActiveDepartureDate.ToLower() != "all")
            {
                DateTime selectedDate = DateTime.Parse(model.ActiveDepartureDate);

                query = query.Where(r => r.Date.Date == selectedDate.Date);
            }
            
            if (!string.IsNullOrEmpty(model.ActiveCabinType) && model.ActiveCabinType.ToLower() != "all")
            {
                query = query.Where(r => r.CabinType == model.ActiveCabinType);
            }

            model.CabinTypes = _context.Flight
                                .Select(f => f.CabinType)
                                .Distinct()
                                .ToList();
            ViewBag.FromCities = _context.Flight
                                .Select(f => f.From)
                                .Distinct()
                                .ToList();
            ViewBag.ToCities = _context.Flight
                                .Select(f => f.To)
                                .Distinct()
                                .ToList();
            model.Flight = query.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Booking(int id)
        {
            var session = new FlightSessions(HttpContext.Session);
            var cookies = new FlightCookies(Request.Cookies, Response.Cookies);

            var flightBooking = new FlightBooking
            {
                FlightId = id,
            };

            _context.FlightBookings.Add(flightBooking);
            _context.SaveChanges();

            var myBookings = session.GetMyBookings();
            myBookings.Add(flightBooking);
            session.SetMyBookings(myBookings);
            cookies.SetMyBookingIds(myBookings);

            TempData["Message"] = "Booking successful! Your ticket has been confirmed.";

            return RedirectToAction("Index", new
            {
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            });
        }

        public IActionResult MyBookings()
        {
            var session = new FlightSessions(HttpContext.Session);
            var cookies = new FlightCookies(Request.Cookies, Response.Cookies);
            var bookingIds = cookies.GetMyBookingIds();
            var bookings = _context.FlightBookings
                .Include(r => r.Flight)
                .Where(r => bookingIds.Contains(r.FlightBookingId.ToString()))
                .ToList();

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
        public IActionResult CancelAllBookings()
        {
            var session = new FlightSessions(HttpContext.Session);

            // Get all bookings from session
            var myBookings = session.GetMyBookings();

            if (myBookings != null && myBookings.Any())
            {
                var ids = myBookings.Select(b => b.FlightBookingId).ToList();

                // Remove from database
                var bookings = _context.FlightBookings
                    .Where(b => ids.Contains(b.FlightBookingId))
                    .ToList();

                _context.FlightBookings.RemoveRange(bookings);
                _context.SaveChanges();

                // Clear session
                session.SetMyBookings(new List<FlightBooking>());

                // Clear cookies
                var cookies = new FlightCookies(Request.Cookies, Response.Cookies);
                foreach (var id in ids)
                {
                    cookies.RemoveBookingId(id);
                }
            }

            TempData["Message"] = "All bookings cancelled successfully!";
            return RedirectToAction("MyBookings");
        }

        [HttpPost]
        public IActionResult CancelBooking(int id)
        {
            var session = new FlightSessions(HttpContext.Session);
            var booking = _context.FlightBookings.Find(id);
            if (booking != null)
            {
                _context.FlightBookings.Remove(booking);
                _context.SaveChanges();
            }

            var myBookings = session.GetMyBookings();
            var bookingInSession = myBookings.FirstOrDefault(r => r.FlightBookingId == id);
            if (bookingInSession != null)
            {
                myBookings.Remove(bookingInSession);
                session.SetMyBookings(myBookings);
            }

            var cookies = new FlightCookies(Request.Cookies, Response.Cookies);
            cookies.RemoveBookingId(id);

            TempData["Message"] = "Ticket cancelled successfully!";
            return RedirectToAction("MyBookings");
        }


        public IActionResult Details(int id)
        {
            var flight = _context.Flight
                .Include(r => r.Airline)
                .FirstOrDefault(r => r.FlightId == id);
            if (flight == null)
                return NotFound();

            var session = new FlightSessions(HttpContext.Session);

            var viewModel = new FlightsViewModel
            {
                Flights = flight,
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            };

            return View(viewModel);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
