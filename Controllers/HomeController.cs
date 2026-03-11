using System.Diagnostics;
using Group6Flight.Models;
using Microsoft.AspNetCore.Mvc;

namespace Group6Flight.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
