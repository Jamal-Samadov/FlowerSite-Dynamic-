using Microsoft.AspNetCore.Mvc;

namespace FlowerSite.Areas.admin.Controllers
{

    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
