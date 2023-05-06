using KeepTrack.Core.DTOs;
using KeepTrack.DataLayer.Models;

namespace KeepTrack.Core.Services.Interfaces
{
    public interface IUserService
    {

        ApplicationUser GetUserByUserId(string userId);

        ApplicationUser GetUserByUserEmail(string email);

        string GetUserPasswordByEmail(string email);

        bool IsUserExistByUserId(string userId);

        bool IsUserExistByUserEmail(string email);

        bool IsUserExistForLogin(string email, string password);

        bool IsUserActive(string userEmail);

        string GetCurrentUserId();

        #region Admin Panel

        UsersForAdminViewModel GetUsers(int take = 10, int pageId = 1, string filterEmail = "", string filterUserName = "");

        UsersForAdminViewModel GetDeletedUsers(int take = 10, int pageId = 1, string filterEmail = "", string filterUserName = "");

        CreateUserViewModel GetUserInfoForCreateUser();

        void AddUserFromAdmin(CreateUserViewModel user);

        EditUserViewModel GetUserInfoForEditUser(string userId);

        void EditUserFromAdmin(EditUserViewModel user);

        DeleteUserViewModel GetUserInfoForDeleteUser(string userId);

        void DeleteUserFromAdmin(string userId);

        RestoreUserViewModel GetUserInfoForRestoreUser(string userId);

        void RestoreUserFromAdmin(string userId);

        #endregion

    }
}
