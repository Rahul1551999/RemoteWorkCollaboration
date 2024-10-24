using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace RemoteWorkCollaboration.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
