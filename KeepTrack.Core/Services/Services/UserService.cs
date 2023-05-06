using KeepTrack.Core.DTOs;
using KeepTrack.Core.Services.Interfaces;
using KeepTrack.DataLayer.Context;
using KeepTrack.DataLayer.Models;
using Microsoft.AspNet.Identity;
using ProjectCMS.Core.Services.Interfaces;
using System.Linq;
using System.Web;

namespace KeepTrack.Core.Services.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;
        private readonly ApplicationUserManager _userManager;

        public UserService(ApplicationUserManager userManager)
        {
            _context = new ApplicationDbContext();
            _roleService = new RoleService();
            _userManager = userManager;
        }

        public ApplicationUser GetUserByUserId(string userId)
        {
            return _userManager.Users.FirstOrDefault(p => p.Id == userId);
        }

        public ApplicationUser GetUserByUserEmail(string email)
        {
            return _userManager.Users.FirstOrDefault(p => p.Email == email);
        }

        public string GetUserPasswordByEmail(string email)
        {
            return _userManager.Users.SingleOrDefault(u => u.Email == email)?.PasswordHash;
        }

        public bool IsUserExistByUserId(string userId)
        {
            return _userManager.Users.Any(u => u.Id == userId);
        }

        public bool IsUserExistByUserEmail(string email)
        {
            return _userManager.Users.Any(u => u.Email == email);

        }

        public bool IsUserExistForLogin(string email, string password)
        {
            var userPass = GetUserPasswordByEmail(email);
            if (userPass != null)
            {
                var Check = _userManager.PasswordHasher.VerifyHashedPassword(userPass, password);
                if (Check == PasswordVerificationResult.Success || Check == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsUserActive(string userEmail)
        {
            var active = _userManager.Users.SingleOrDefault(p => p.Email == userEmail);
            if (active != null)
            {
                return active.Active;
            }
            return false;
        }

        public string GetCurrentUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }

        public UsersForAdminViewModel GetUsers(int take = 10, int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<ApplicationUser> result = _userManager.Users.Where(u => u.Active);

            if (!string.IsNullOrWhiteSpace(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrWhiteSpace(filterUserName))
            {
                result = result.Where(u => u.Name.Contains(filterUserName));
            }

            // Show item in page
            int skip = (pageId - 1) * take;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count();
            list.Users = result.OrderBy(u => u.Id).Skip(skip).Take(take).ToList().Select(p => new UsersAdmin()
            {
                User = p,
                RoleName = _context.Roles.SingleOrDefault(r => r.Id == p.RoleId.ToString())?.Name
            }).ToList();
            _userManager.Dispose();
            return list;
        }

        public UsersForAdminViewModel GetDeletedUsers(int take = 10, int pageId = 1, string filterEmail = "",
            string filterUserName = "")
        {
            IQueryable<ApplicationUser> result = _userManager.Users.Where(u => !u.Active);

            if (!string.IsNullOrWhiteSpace(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrWhiteSpace(filterUserName))
            {
                result = result.Where(u => u.Name.Contains(filterUserName));
            }

            // Show item in page
            int skip = (pageId - 1) * take;

            UsersForAdminViewModel list = new UsersForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count();
            list.Users = result.OrderByDescending(u => u.Id).Skip(skip).Take(take).ToList().Select(p => new UsersAdmin()
            {
                User = p,
                RoleName = _context.Roles.SingleOrDefault(r => r.Id == p.RoleId.ToString())?.Name
            }).ToList();

            return list;
        }

        public CreateUserViewModel GetUserInfoForCreateUser()
        {
            return new CreateUserViewModel()
            {
                SelectedRole = 4,
                Roles = _roleService.GetAllRoles()
            };
        }

        public void AddUserFromAdmin(CreateUserViewModel user)
        {
            ApplicationUser createUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                Name = user.UserName,
                Active = true,
                RoleId = user.SelectedRole
            };
            _userManager.Create(createUser, user.Password);
        }

        public EditUserViewModel GetUserInfoForEditUser(string userId)
        {
            var res = _userManager.Users.Where(u => u.Id == userId).Select(u => new EditUserViewModel
            {
                UserId = userId,
                UserName = u.Name,
                Email = u.Email,
                SelectedRole = u.RoleId,
                Active = u.Active
            }).Single();
            res.Roles = _roleService.GetAllRoles();
            return res;
        }

        public void EditUserFromAdmin(EditUserViewModel user)
        {
            ApplicationUser editUser = GetUserByUserId(user.UserId);
            //set editUser to user
            {
                if (user.Password != null)
                {
                    editUser.PasswordHash = _userManager.PasswordHasher.HashPassword(user.Password);
                }

                editUser.Email = user.Email;
                editUser.Name = user.UserName;
                editUser.RoleId = user.SelectedRole;
                _userManager.Update(editUser);
            }

        }

        public DeleteUserViewModel GetUserInfoForDeleteUser(string userId)
        {
            var res = _userManager.Users.Where(u => u.Id == userId)
                .Select(u => new DeleteUserViewModel
                {
                    RoleId = u.RoleId,
                    Email = u.Email,
                    Name = u.Name
                }).SingleOrDefault();

            var roleName = _context.Roles.FirstOrDefault(p => p.Id == res.RoleId.ToString())?.Name;
            if (res != null)
            {
                res.RoleName = roleName;
            }
            return res;
        }


        public void DeleteUserFromAdmin(string userId)
        {
            var user = GetUserByUserId(userId);
            user.Active = false;
            _userManager.Update(user);
        }

        public RestoreUserViewModel GetUserInfoForRestoreUser(string userId)
        {
            var res = _userManager.Users.Where(u => u.Id == userId)
                .Select(u => new RestoreUserViewModel
                {
                    RoleId = u.RoleId,
                    Email = u.Email,
                    Name = u.Name
                }).SingleOrDefault();

            var roleName = _context.Roles.FirstOrDefault(p => p.Id == res.RoleId.ToString())?.Name;
            if (res != null)
            {
                res.RoleName = roleName;
            }
            return res;
        }


        public void RestoreUserFromAdmin(string userId)
        {
            var user = GetUserByUserId(userId);
            user.Active = true;
            _userManager.Update(user);

        }
    }

}
