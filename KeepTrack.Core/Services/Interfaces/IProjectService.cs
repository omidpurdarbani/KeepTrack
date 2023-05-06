using KeepTrack.Core.DTOs;
using KeepTrack.DataLayer.Models;
using System.Collections.Generic;

namespace KeepTrack.Core.Services.Interfaces
{
    public interface IProjectService
    {
        List<Project> GetUserAllProjects(string userId);
        List<UserAllProjectsViewModel> GetInfoForUserAllProjectsViewModel(string userId);

        Project GetUserProjectById(string userId, int projectId);
        UserProjectViewModel GetInfoForUserProject(string userId, int projectId);

        ProjectTask GetProjectTaskById(int taskId);

        List<ProjectTask> GetProjectTasksForUserProject(int projectId);

        List<StateType> GetAllStates();
        List<ProjectType> GetAllProjectTypes();

        InsertTaskViewModel GetInfoForInsertTask(int projectId, int parentId);

        void InsertTask(ProjectTask task);

        EditTaskViewModel GetInfoForEditTask(int taskId);

        void EditTask(ProjectTask task);

        DeleteTaskViewModel GetInfoForDeleteTask(int taskId);

        void DeleteTask(ProjectTask task);

        EditUserProjectViewModel GetInfoForEditUserProject(int projectId);

        void EditProject(Project project);

        //Admin panel
        ProjectsForAdminViewModel GetProjects(int take = 10, int pageId = 1, string filterName = "", string filterUserName = "");

        ProjectsForAdminViewModel GetDeletedProjects(int take = 10, int pageId = 1, string filterName = "", string filterUserName = "");

        CreateProjectViewModel GetInfoForCreateProject(string userId);

        void InsertProject(Project project);

        RestoreOrDeleteProjectViewModel GetInfoForDeleteProject(int projectId);

        void DeleteProject(Project project);

        void RestoreProject(Project project);

    }
}
