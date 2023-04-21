using KeepTrack.Core.Security;
using System.Web.Mvc;

namespace KeepTrack.Web.Areas.Admin.Controllers
{
    [RoleChecker(new[] { 1 })]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}