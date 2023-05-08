using KeepTrack.Core.DTOs;
using KeepTrack.Core.Services.Interfaces;
using KeepTrack.Core.Services.Services;
using KeepTrack.DataLayer.Models;
using Microsoft.AspNet.Identity;
using ProjectCMS.Core.Services.Interfaces;
using System;
using System.Web.Mvc;

namespace KeepTrack.Web.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IRoleService _roleService;

        public ProjectsController()
        {
            _projectService = new ProjectService();
            _roleService = new RoleService();
        }

        // GET: Projects
        public ActionResult Index(int projectId)
        {
            string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            return View(_projectService.GetInfoForUserProject(userId, projectId));
        }


        public ActionResult EditProject(string projectId)
        {
            if (_roleService.GetCurrentUserRoleId() == 1)
            {
                ViewBag.admin = true;
            }

            var model = _projectService.GetInfoForEditUserProject(Int32.Parse(projectId));
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(EditUserProjectViewModel project, int stateTypeId, string userId, int projectTypeId)
        {
            _projectService.EditProject(new Project()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                ProjectTypeId = projectTypeId,
                StateTypeId = stateTypeId,
                UserId = userId,
                Active = project.Active
            });
            return RedirectToAction("Index", "Projects", new { @projectId = project.Id });
        }

        [ChildActionOnly]
        public ActionResult ShowProjectTasks(int projectId)
        {
            var roleId = _roleService.GetCurrentUserRoleId();
            var tasks = _projectService.GetProjectTasksForUserProject(projectId);
            if (roleId == 1 || roleId == 2)
            {
                ViewBag.ProjectId = projectId;
                return PartialView("ShowProjectTasksCRUD", tasks);
            }
            return PartialView(tasks);
        }

        public ActionResult InsertTask(string projectId, string parentId)
        {
            return PartialView(_projectService.GetInfoForInsertTask(Int32.Parse(projectId), Int32.Parse(parentId)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertTask(InsertTaskViewModel task, int stateTypes)
        {
            _projectService.InsertTask(new ProjectTask()
            {
                Name = task.Name,
                ParentId = task.ParentId,
                ProjectId = task.ProjectId,
                StateTypeId = stateTypes
            });
            return RedirectToAction("Index", "Projects", new { @projectId = task.ProjectId });
        }

        public ActionResult EditTask(int taskId)
        {
            return PartialView(_projectService.GetInfoForEditTask(taskId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTask(EditTaskViewModel task, int stateTypes)
        {
            _projectService.EditTask(new ProjectTask()
            {
                Id = task.TaskId,
                Name = task.Name,
                StateTypeId = stateTypes,
                ProjectId = task.ProjectId,
                ParentId = task.ParentId
            });
            return RedirectToAction("Index", "Projects", new { @projectId = task.ProjectId });
        }

        public ActionResult DeleteTask(int taskId)
        {
            return PartialView(_projectService.GetInfoForDeleteTask(taskId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTask(int taskId, int projectId)
        {

            _projectService.DeleteTask(_projectService.GetProjectTaskById(taskId));

            return RedirectToAction("Index", "Projects", new { @projectId = projectId });
        }
    }
}