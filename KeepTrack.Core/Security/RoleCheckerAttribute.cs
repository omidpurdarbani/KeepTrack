using Microsoft.AspNet.Identity;
using ProjectCMS.Core.Services.Services;
using System.Web.Mvc;

namespace KeepTrack.Core.Security
{
    public class RoleCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private int[] roleId;
        private RoleService roleService;

        public RoleCheckerAttribute(int[] roleId)
        {
            this.roleId = roleId;
            roleService = new RoleService();
        }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!roleService.CheckRole
                        (
                            roleId,
                            filterContext.HttpContext.User.Identity.GetUserId())
                    )
                {
                    filterContext.Result = new RedirectResult("/account/login");
                }

            }
            else
            {
                filterContext.Result = new RedirectResult("/account/login");
            }
        }
    }
}
