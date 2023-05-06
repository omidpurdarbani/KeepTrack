using KeepTrack.Core.Convertors;
using KeepTrack.Core.DTOs;
using KeepTrack.Core.Security;
using KeepTrack.Core.Services.Services;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace KeepTrack.Web.Areas.Admin.Controllers
{
    [RoleChecker(new[] { 1 })]
    public class UsersController : Controller
    {
        private static RoleService RoleService => new RoleService();
        private UserService UserService => new UserService(UserManager);

        private ApplicationUserManager _userManager;
        private ApplicationUserManager UserManager
        {
            get => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            set => _userManager = value;
        }

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager manager)
        {
            UserManager = manager;
        }

        // GET: Admin/Users
        public ActionResult Index(int pageId = 1, string filterEmail = "", string filterUsername = "")
        {

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["filterUsername"]))
            {
                ViewData["name"] = HttpContext.Request.QueryString["filterUsername"];
            }
            if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["filterEmail"]))
            {
                ViewData["email"] = HttpContext.Request.QueryString["filterEmail"];
            }
            return View(UserService.GetUsers(10, pageId, filterEmail, filterUsername));
        }

        // GET: Admin/Users/Deleted
        public ActionResult Deleted(int pageId = 1, string filterEmail = "", string filterUsername = "")
        {

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["filterUsername"]))
            {
                ViewData["name"] = HttpContext.Request.QueryString["filterUsername"];
            }
            if (!string.IsNullOrWhiteSpace(HttpContext.Request.QueryString["filterEmail"]))
            {
                ViewData["email"] = HttpContext.Request.QueryString["filterEmail"];
            }
            return View(UserService.GetDeletedUsers(10, pageId, filterEmail, filterUsername));
        }

        private static CreateUserViewModel CreateUserViewModel { get; set; }

        // GET: admin/users/create
        public ActionResult Create()
        {
            CreateUserViewModel = UserService.GetUserInfoForCreateUser();
            return View(CreateUserViewModel);
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel model)
        {
            CreateUserViewModel = model;
            CreateUserViewModel.Roles = RoleService.GetAllRoles();
            if (!ModelState.IsValid)
            {
                return View(CreateUserViewModel);
            }
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("UserName", "لطفا نام کاربری را وارد کنید");
                return View(CreateUserViewModel);
            }
            if (UserService.IsUserExistByUserEmail(CreateUserViewModel.Email.FixEmail()))
            {
                ModelState.AddModelError("Email", "ایمیل قبلا ثبت شده می باشد !");
                return View(CreateUserViewModel);
            }


            UserService.AddUserFromAdmin(CreateUserViewModel);

            return Redirect("/Admin/Users");
        }

        private static EditUserViewModel EditUserViewModel { get; set; }

        // GET: Admin/User/Edit
        public ActionResult Edit(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                EditUserViewModel = UserService.GetUserInfoForEditUser(userId);
                return View(EditUserViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel model)
        {
            EditUserViewModel.Roles = RoleService.GetAllRoles();

            if (EditUserViewModel.Email != model.Email)
            {
                if (UserService.IsUserExistByUserEmail(model.Email.FixEmail()))
                {
                    ModelState.AddModelError("Email", "ایمیل قبلا ثبت شده می باشد !");
                    return View(model);
                }
            }

            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("UserName", "لطفا نام کاربری را وارد کنید");
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            /*
                        if (!UserManager.CheckPassword(UserService.GetUserByUserId(EditUserViewModel.UserId), EditUserViewModel.Password))
                        {
                            ModelState.AddModelError("Password", "رمز عبور مناسب نمی باشد");
                            return View(EditUserViewModel);
                        }*/


            UserService.EditUserFromAdmin(model);
            if (EditUserViewModel.Active)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Deleted");
        }

        private static string DeleteUserId { get; set; }

        // GET: Admin/User/Delete
        public ActionResult Delete(string userId)
        {
            DeleteUserViewModel deleteUser;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Redirect("/admin/users/");
            }

            if (UserService.IsUserExistByUserId(userId))
            {
                deleteUser = UserService.GetUserInfoForDeleteUser(userId);
            }
            else
            {
                return Redirect("/admin/users/");
            }

            DeleteUserId = userId;

            return View(deleteUser);
        }

        [HttpPost]
        public ActionResult Delete()
        {
            UserService.DeleteUserFromAdmin(DeleteUserId);
            return RedirectToAction("Index");
        }

        private static string RestoreUserId { get; set; }

        // GET: Admin/User/Delete?userId={userId}
        public ActionResult Restore(string userId)
        {
            RestoreUserViewModel restoreUser;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Redirect("/admin/users/");
            }

            if (UserService.IsUserExistByUserId(userId))
            {
                restoreUser = UserService.GetUserInfoForRestoreUser(userId);
            }
            else
            {
                return Redirect("/admin/users/");
            }

            RestoreUserId = userId;

            return View(restoreUser);
        }

        [HttpPost]
        public ActionResult Restore()
        {
            UserService.RestoreUserFromAdmin(RestoreUserId);
            return RedirectToAction("Deleted");
        }
    }
}