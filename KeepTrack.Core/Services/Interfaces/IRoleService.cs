using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace ProjectCMS.Core.Services.Interfaces
{
    public interface IRoleService
    {
        int GetCurrentUserRoleId(string userId = null);
        bool CheckRole(int[] roleId);
        List<IdentityRole> GetAllRoles();
    }
}
