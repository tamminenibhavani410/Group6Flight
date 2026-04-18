using Group6Flight.Models;
using Microsoft.AspNetCore.Mvc;

namespace Group6Flight.Controllers
{
    public class ValidationController : Controller
    {
        private FlightDbContext context;
        public ValidationController(FlightDbContext ctx) => context = ctx;

        public JsonResult CheckFlightCodeDate(string FlightCode, DateTime Date)
        {
            string msg = Check.DateFlightCodeCombo(context, Date, FlightCode);  
            if (string.IsNullOrEmpty(msg))
            {
                TempData["okFlightCodeDate"] = true;
                return Json(true);
            }
            else return Json(msg);
        }
    }
}
