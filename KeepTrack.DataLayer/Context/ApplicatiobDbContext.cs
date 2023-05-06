using KeepTrack.DataLayer.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace KeepTrack.DataLayer.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<StateType> StateTypes { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}
