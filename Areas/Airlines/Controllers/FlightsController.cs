using Group6Flight.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group6Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class FlightsController : Controller
    {
        private FlightDbContext context { get; set; }

        public FlightsController(FlightDbContext ctx) => context = ctx;

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            var airlines = context.Airline
                .OrderBy(m => m.AirlineId).ToList();
            ViewBag.Airlines = airlines
                .Select(l => new SelectListItem
                {
                    Value = l.AirlineId.ToString(),
                    Text = l.Name
                })
                .ToList();
            return View("Edit", new Flight());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Disable = "";
            var airlines = context.Airline
                .OrderBy(m => m.AirlineId).ToList();
            ViewBag.Airlines = airlines
                .Select(l => new SelectListItem
                {
                    Value = l.AirlineId.ToString(),
                    Text = l.Name
                })
                .ToList();
            var flight = context.Flight.Find(id);
            return View(flight);
        }

        [HttpPost]
        public IActionResult Edit(Flight flight)
        {
            if (TempData["okFlightCodeDate"] == null)
            {
                string msg = Check.DateFlightCodeCombo(context, flight.Date, flight.FlightCode);
                if (!String.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(nameof(flight.FlightCode), msg);
                }
            }
            if (ModelState.IsValid)
            {
                if (flight.FlightId == 0)
                {
                    context.Flight.Add(flight);
                    TempData["Message"] = $"{flight.FlightCode} Added Successfully";
                }
                else
                {
                    context.Flight.Update(flight);
                    TempData["Message"] = $"{flight.FlightCode} updated successfully.";
                }
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var airlines = context.Airline
                .OrderBy(m => m.AirlineId).ToList();
                ViewBag.Airlines = airlines
                    .Select(l => new SelectListItem
                    {
                        Value = l.AirlineId.ToString(),
                        Text = l.Name
                    })
                    .ToList();
                ViewBag.Action = (flight.FlightId == 0) ? "Add" : "Edit";
                return View(flight);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var flight = context.Flight.Find(id);
            return View(flight);
        }

        [HttpPost]
        public IActionResult Delete(Flight flight)
        {
            context.Flight.Remove(flight);
            context.SaveChanges();
            TempData["Message"] = $"{flight.FlightCode} Deleted Successfully";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ManageFlights()
        {
            return Content($"Area: [Airlines], Controller: Flights, Action: ManageFlights");
        }
        
        public IActionResult Regulation()
        {
            return Content($"Area: [Airlines], Controller: Flights, Action: Regulation");
        }
    }
}
