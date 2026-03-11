using Microsoft.AspNetCore.Mvc;

namespace Group6Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class FlightsController : Controller
    {
        public IActionResult Manage()
        {
            return View();
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
