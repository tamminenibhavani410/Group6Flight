using Microsoft.AspNetCore.Mvc;

namespace Group6Flight.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageUsers()
        {
            return Content($"Area: [Airlines], Controller: Users, Action: ManageUsers");
        }

        public IActionResult RightsObligations()
        {
            return Content($"Area: [Airlines], Controller: Users, Action: RightsObligations");
        }
    }
}
