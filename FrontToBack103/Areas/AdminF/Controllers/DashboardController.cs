using Microsoft.AspNetCore.Mvc;

namespace FrontToBack103.Areas.AdminF.Controllers
{
    [Area("AdminF")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
