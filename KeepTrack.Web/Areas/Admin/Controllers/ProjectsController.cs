using KeepTrack.Core.DTOs;
using KeepTrack.Core.Security;
using KeepTrack.Core.Services.Interfaces;
using KeepTrack.Core.Services.Services;
using KeepTrack.DataLayer.Models;
using System.Web.Mvc;

namespace KeepTrack.Web.Areas.Admin.Controllers
{
    [RoleChecker(new[] { 1 })]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController()
        {
            _projectService = new ProjectService();
        }

        // GET: Admin/Projects
        public ActionResult Index(int pageId = 1, string filterName = "", string filterUsername = "")
        {
            if (!string.IsNullOrWhiteSpace(filterUsername))
            {
                ViewData["username"] = filterUsername;
            }
            if (!string.IsNullOrWhiteSpace(filterName))
            {
                ViewData["name"] = filterName;
            }
            return View(_projectService.GetProjects(10, pageId, filterName, filterUsername));
        }

        // GET: Admin/Projects/Deleted
        public ActionResult Deleted(int pageId = 1, string filterName = "", string filterUsername = "")
        {
            if (!string.IsNullOrWhiteSpace(filterUsername))
            {
                ViewData["username"] = filterUsername;
            }
            if (!string.IsNullOrWhiteSpace(filterName))
            {
                ViewData["name"] = filterName;
            }
            return View(_projectService.GetDeletedProjects(10, pageId, filterName, filterUsername));
        }


        public ActionResult Insert(string userId)
        {
            return PartialView(_projectService.GetInfoForCreateProject(userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(CreateProjectViewModel project, int projectStateId, int projectTypeId)
        {
            _projectService.InsertProject(new Project()
            {
                Name = project.Name,
                Description = project.Description,
                ProjectTypeId = projectTypeId,
                StateTypeId = projectTypeId,
                UserId = project.UserId,
                Active = true
            });
            return Redirect("/admin/projects");
        }

        public ActionResult Delete(string projectId)
        {
            return View(_projectService.GetInfoForDeleteProject(int.Parse(projectId)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(RestoreOrDeleteProjectViewModel model)
        {
            _projectService.DeleteProject(model.Id);
            return Redirect("/admin/projects");
        }

        public ActionResult Restore(string projectId)
        {
            return View(_projectService.GetInfoForDeleteProject(int.Parse(projectId)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(RestoreOrDeleteProjectViewModel model)
        {
            _projectService.RestoreProject(model.Id);
            return Redirect("/admin/projects/deleted");
        }

    }
}