using KeepTrack.DataLayer.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace KeepTrack.Core.DTOs
{
    #region Admin

    public class ProjectsForAdminViewModel
    {
        public List<Project> Projects { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class CreateProjectViewModel
    {
        public string UserId { get; set; }

        [DisplayName("نام")]
        public string Name { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        public int ProjectTypeId { get; set; }

        public int ProjectStateId { get; set; }

        [DisplayName("وضعیت")]
        public List<StateType> StateTypes { get; set; }

        [DisplayName("دسته بندی")]
        public List<ProjectType> ProjectTypes { get; set; }
    }

    public class RestoreOrDeleteProjectViewModel
    {
        public int Id { get; set; }

        [DisplayName("نام")]
        public string Name { get; set; }

        [DisplayName("نام کاربر")]
        public string UserName { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        public string ProjectTypeName { get; set; }

        public string ProjectStateName { get; set; }
    }

    #endregion

    public class UserAllProjectsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DataName { get; set; }

        public string Description { get; set; }

        public string ProjectTypeImage { get; set; }
    }

    public class UserProjectViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string StateTypeName { get; set; }

        public string ProjectTypeName { get; set; }


    }

    public class EditUserProjectViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int StateTypeId { get; set; }

        public int ProjectTypeId { get; set; }

        [DisplayName("وضعیت")]
        public List<StateType> StateTypes { get; set; }

        [DisplayName("کاربران")]
        public List<ApplicationUser> Users { get; set; }

        [DisplayName("دسته بندی")]
        public List<ProjectType> ProjectTypes { get; set; }

    }

    public class InsertTaskViewModel
    {
        [DisplayName("نام")]
        public string Name { get; set; }

        [DisplayName("وضعیت")]
        public List<StateType> StateTypes { get; set; }

        public int ParentId { get; set; }

        public int ProjectId { get; set; }
    }
    public class EditTaskViewModel
    {
        public int TaskId { get; set; }

        [DisplayName("نام")]
        public string Name { get; set; }

        [DisplayName("وضعیت")]
        public List<StateType> StateTypes { get; set; }

        [DisplayName("وضعیت")]
        public int StateTypeId { get; set; }

        public int ParentId { get; set; }

        public int ProjectId { get; set; }
    }

    public class DeleteTaskViewModel
    {
        public int TaskId { get; set; }

        [DisplayName("نام")]
        public string Name { get; set; }

        [DisplayName("وضعیت")]
        public List<StateType> StateTypes { get; set; }

        [DisplayName("وضعیت")]
        public int StateTypeId { get; set; }

        public int ProjectId { get; set; }
    }
}
