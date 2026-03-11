using Microsoft.AspNetCore.Mvc;

namespace Group6Flight.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return Content($"Controller: Search, Action: Index");
        }
    }
}
