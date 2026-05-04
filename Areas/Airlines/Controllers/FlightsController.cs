using Group6Flight.Models;
using Group6Flight.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group6Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class FlightsController : Controller
    {
        private IFlightRepository flightRepo;
        private IRepository<Airline> airlineRepo;
        private IBookingRepository bookingRepo;

        public FlightsController(
            IFlightRepository fRepo,
            IRepository<Airline> aRepo,
            IBookingRepository bRepo)
        {
            flightRepo = fRepo;
            airlineRepo = aRepo;
            bookingRepo = bRepo;
        }

        // GET: Add
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            LoadAirlines();
            return View("Edit", new Flight());
        }

        // GET: Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Disable = "";

            LoadAirlines();

            var flight = flightRepo.Get(id);
            return View(flight);
        }

        // POST: Save (Add/Edit)
        [HttpPost]
        public IActionResult Edit(Flight flight)
        {
            if (TempData["okFlightCodeDate"] == null)
            {
                bool exists = flightRepo.FlightCodeDateExists(
                    flight.FlightCode,
                    flight.Date);

                if (exists && flight.FlightId == 0)
                {
                    ModelState.AddModelError(
                        nameof(flight.FlightCode),
                        "Flight code already exists for this date.");
                }
            }

            if (ModelState.IsValid)
            {
                if (flight.FlightId == 0)
                {
                    flightRepo.Insert(flight);
                    TempData["Message"] = $"{flight.FlightCode} Added Successfully";
                }
                else
                {
                    flightRepo.Update(flight);
                    TempData["Message"] = $"{flight.FlightCode} updated successfully.";
                }

                flightRepo.Save();

                return RedirectToAction("Index", "Home");
            }

            LoadAirlines();
            ViewBag.Action = (flight.FlightId == 0) ? "Add" : "Edit";

            return View(flight);
        }

        // GET: Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var flight = flightRepo.Get(id);
            return View(flight);
        }

        // POST: Delete
        [HttpPost]
        public IActionResult Delete(Flight flight)
        {
            bool hasReservations = bookingRepo.List(
                new QueryOptions<FlightBooking>())
                .Any(b => b.FlightId == flight.FlightId);

            if (hasReservations)
            {
                TempData["Message"] =
                    $"Cannot delete {flight.FlightCode}. " +
                    $"This flight has existing reservations.";

                return RedirectToAction("Index", "Home");
            }

            flightRepo.Delete(flight);
            flightRepo.Save();

            TempData["Message"] =
                $"{flight.FlightCode} Deleted Successfully";

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ManageFlights()
        {
            return Content("Area: [Airlines], Controller: Flights, Action: ManageFlights");
        }

        public IActionResult Regulation()
        {
            return Content("Area: [Airlines], Controller: Flights, Action: Regulation");
        }

        // dropdown helper
        private void LoadAirlines()
        {
            var airlines = airlineRepo.List(new QueryOptions<Airline>())
                .OrderBy(a => a.AirlineId)
                .Select(a => new SelectListItem
                {
                    Value = a.AirlineId.ToString(),
                    Text = a.Name
                })
                .ToList();

            ViewBag.Airlines = airlines;
        }
    }
}