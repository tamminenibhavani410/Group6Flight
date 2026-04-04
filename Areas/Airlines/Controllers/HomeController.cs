using Group6Flight.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group6Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class HomeController : Controller
    {
        private FlightDbContext context { get; set; }

        public HomeController(FlightDbContext ctx) => context = ctx;

        public IActionResult Index()
        {
            var flights = context.Flight
                .Include(r => r.Airline)
                .OrderBy(m => m.FlightCode).ToList();
            return View(flights);
        }
    }
}
