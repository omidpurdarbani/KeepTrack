using KeepTrack.Core.Services.Interfaces;
using KeepTrack.Core.Services.Services;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace KeepTrack.Web.Controllers
{
    public class HomeController : Controller
    {
        private IProjectService _projectService;

        public HomeController()
        {
            _projectService = new ProjectService();
        }
        public ActionResult Index()
        {
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            return View(
                _projectService.GetInfoForUserAllProjectsViewModel(userId)
            );
        }
    }
}