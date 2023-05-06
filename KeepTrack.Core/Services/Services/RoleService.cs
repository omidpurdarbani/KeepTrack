using KeepTrack.DataLayer.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjectCMS.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KeepTrack.Core.Services.Services
{
    public class RoleService : IRoleService
    {
        private ApplicationDbContext _context;

        public RoleService()
        {
            _context = new ApplicationDbContext();
        }

        public int GetCurrentUserRoleId(string userId = null)
        {
            if (userId == null)
            {
                userId = HttpContext.Current.User.Identity.GetUserId();
            }

            return _context.Users.AsNoTracking()
                .Single(u => u.Id == userId)
                .RoleId;
        }

        public bool CheckRole(int[] roleId)
        {
            var userRole = Convert.ToInt32(GetCurrentUserRoleId());

            return roleId.Contains(userRole);
        }

        public List<IdentityRole> GetAllRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
