using Microsoft.AspNetCore.Mvc;

namespace RestaurantInfrastructure.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}