using KeepTrack.Core.DTOs;
using KeepTrack.Core.Services.Interfaces;
using KeepTrack.DataLayer.Context;
using KeepTrack.DataLayer.Models;
using ProjectCMS.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace KeepTrack.Core.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;

        public ProjectService()
        {
            _context = new ApplicationDbContext();
            _roleService = new RoleService();
        }

        public List<Project> GetUserAllProjects(string userId)
        {
            return _context.Projects.Where(p => p.UserId == userId).ToList();
        }

        public List<UserAllProjectsViewModel> GetInfoForUserAllProjectsViewModel(string userId)
        {
            return _context.Projects.Include(p => p.ProjectType).Where(p => p.UserId == userId).Select(p => new UserAllProjectsViewModel()
            {
                Id = p.Id,
                Description = p.Description,
                Name = p.Name,
                DataName = p.ProjectType.DataName,
                ProjectTypeImage = p.ProjectType.Image
            }).ToList();
        }

        public Project GetUserProjectById(string userId, int projectId)
        {
            return _context.Projects.SingleOrDefault(p => p.UserId == userId && p.Id == projectId);
        }

        public UserProjectViewModel GetInfoForUserProject(string userId, int projectId)
        {
            var roleId = _roleService.GetCurrentUserRoleId(userId);
            if (roleId == 1)
            {
                return _context.Projects.Include(p => p.ProjectType).Include(p => p.StateType).Include(p => p.ProjectTasks).Select(p => new UserProjectViewModel()
                {
                    Description = p.Description,
                    Id = p.Id,
                    Name = p.Name,
                    ProjectTypeName = p.ProjectType.Name,
                    StateTypeName = p.StateType.Name,
                    UserId = p.UserId
                }).SingleOrDefault(p => p.Id == projectId);
            }
            return _context.Projects.Include(p => p.ProjectType).Include(p => p.StateType).Include(p => p.ProjectTasks).Select(p => new UserProjectViewModel()
            {
                Description = p.Description,
                Id = p.Id,
                Name = p.Name,
                ProjectTypeName = p.ProjectType.Name,
                StateTypeName = p.StateType.Name,
                UserId = p.UserId
            }).SingleOrDefault(p => p.UserId == userId && p.Id == projectId);
        }

        public ProjectTask GetProjectTaskById(int taskId)
        {
            return _context.ProjectTasks.FirstOrDefault(p => p.Id == taskId);
        }

        public List<ProjectTask> GetProjectTasksForUserProject(int projectId)
        {
            return _context.ProjectTasks.Where(p => p.ProjectId == projectId).ToList();
        }

        public List<StateType> GetAllStates()
        {
            return _context.StateTypes.ToList();
        }

        public List<ProjectType> GetAllProjectTypes()
        {
            return _context.ProjectTypes.ToList();
        }

        public InsertTaskViewModel GetInfoForInsertTask(int projectId, int parentId)
        {
            return new InsertTaskViewModel()
            {
                ProjectId = projectId,
                ParentId = parentId,
                StateTypes = GetAllStates()
            };
        }

        public void InsertTask(ProjectTask task)
        {
            _context.ProjectTasks.AddOrUpdate(task);
            _context.SaveChanges();
        }

        public EditTaskViewModel GetInfoForEditTask(int taskId)
        {
            var res = _context.ProjectTasks.Where(p => p.Id == taskId).Select(p => new EditTaskViewModel()
            {
                TaskId = taskId,
                ProjectId = p.ProjectId,
                Name = p.Name,
                StateTypeId = p.StateTypeId.Value,
                ParentId = p.ParentId
            }).Single();

            res.StateTypes = GetAllStates();
            return res;
        }

        public void EditTask(ProjectTask task)
        {
            _context.ProjectTasks.AddOrUpdate(task);
            _context.SaveChanges();
        }

        public DeleteTaskViewModel GetInfoForDeleteTask(int taskId)
        {
            var res = _context.ProjectTasks.Where(p => p.Id == taskId).Select(p => new DeleteTaskViewModel()
            {
                TaskId = taskId,
                Name = p.Name,
                StateTypeId = p.StateTypeId.Value,
                ProjectId = p.ProjectId
            }).Single();

            res.StateTypes = GetAllStates();
            return res;
        }

        public void DeleteTask(ProjectTask task)
        {
            _context.ProjectTasks.Remove(task);
            _context.SaveChanges();
        }

        public EditUserProjectViewModel GetInfoForEditUserProject(int projectId)
        {
            var res = _context.Projects.Where(p => p.Id == projectId)
                .Include(p => p.ProjectType)
                .Include(p => p.StateType)
                .Select(p => new EditUserProjectViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name,
                    UserId = p.UserId,
                    ProjectTypeId = p.ProjectType.Id,
                    StateTypeId = p.StateType.Id
                }).SingleOrDefault();
            if (res != null)
            {
                res.StateTypes = GetAllStates();
                res.Users = _context.Users.ToList();
                res.ProjectTypes = GetAllProjectTypes();
            }

            return res;
        }

        public void EditProject(Project project)
        {
            _context.Projects.AddOrUpdate(project);
            _context.SaveChanges();
        }

        public ProjectsForAdminViewModel GetProjects(int take = 10, int pageId = 1, string filterName = "",
            string filterUserName = "")
        {
            IQueryable<Project> result = _context.Projects.Where(u => u.Active).Include(p => p.ApplicationUser);

            if (!string.IsNullOrWhiteSpace(filterName))
            {
                result = result.Where(u => u.Name.Contains(filterName));
            }

            if (!string.IsNullOrWhiteSpace(filterUserName))
            {
                result = result.Where(u => u.ApplicationUser.Name.Contains(filterUserName));
            }

            // Show item in page
            int skip = (pageId - 1) * take;

            ProjectsForAdminViewModel list = new ProjectsForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count();
            list.Projects = result.OrderBy(u => u.Id).Skip(skip).Take(take).ToList();

            return list;
        }

        public ProjectsForAdminViewModel GetDeletedProjects(int take = 10, int pageId = 1, string filterName = "",
            string filterUserName = "")
        {
            IQueryable<Project> result = _context.Projects.Where(u => !u.Active).Include(p => p.ApplicationUser);

            if (!string.IsNullOrWhiteSpace(filterName))
            {
                result = result.Where(u => u.Name.Contains(filterName));
            }

            if (!string.IsNullOrWhiteSpace(filterUserName))
            {
                result = result.Where(u => u.ApplicationUser.Name.Contains(filterUserName));
            }

            // Show item in page
            int skip = (pageId - 1) * take;

            ProjectsForAdminViewModel list = new ProjectsForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count();
            list.Projects = result.OrderBy(u => u.Id).Skip(skip).Take(take).ToList();

            return list;
        }

        public CreateProjectViewModel GetInfoForCreateProject(string userId)
        {
            return new CreateProjectViewModel()
            {
                UserId = userId,
                ProjectTypes = GetAllProjectTypes(),
                StateTypes = GetAllStates()
            };
        }

        public void InsertProject(Project project)
        {
            _context.Projects.AddOrUpdate(project);
            _context.SaveChanges();
        }

        public RestoreOrDeleteProjectViewModel GetInfoForDeleteProject(int projectId)
        {
            return _context.Projects.Where(p => p.Id == projectId)
                .Include(p => p.StateType)
                .Include(p => p.ApplicationUser)
                .Include(p => p.ProjectType)
                .Select(p => new RestoreOrDeleteProjectViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name,
                    ProjectTypeName = p.ProjectType.Name,
                    ProjectStateName = p.StateType.Name,
                    UserName = p.ApplicationUser.Name
                }).SingleOrDefault();
        }

        public void DeleteProject(Project project)
        {
            project.Active = false;
            _context.Projects.AddOrUpdate(project);
            _context.SaveChanges();
        }

        public void RestoreProject(Project project)
        {
            project.Active = true;
            _context.Projects.AddOrUpdate(project);
            _context.SaveChanges();
        }
    }
}
