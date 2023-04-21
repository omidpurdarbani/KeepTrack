using KeepTrack.DataLayer.Models;
using ProjectCMS.Core.Services.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;


namespace ProjectCMS.Core.Services.Services
{
    public class RoleService : IRoleService
    {
        private ApplicationDbContext _context;

        public RoleService()
        {
            _context = new ApplicationDbContext();
        }
        public bool CheckRole(int[] roleId, string userId)
        {
            var userRole = _context.Users
                .Include(r => r.Roles)
                .Where(u => u.Id == userId)
                .SelectMany(role => role.Roles).Single().RoleId;

            return roleId.Contains(Convert.ToInt32(userRole));
        }
    }
}
