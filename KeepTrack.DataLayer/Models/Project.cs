using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeepTrack.DataLayer.Models
{
    public class Project
    {
        public Project()
        {

        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int StateTypeId { get; set; }

        public string UserId { get; set; }

        public int ProjectTypeId { get; set; }

        public bool Active { get; set; }

        //Relations

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("ProjectTypeId")]
        public virtual ProjectType ProjectType { get; set; }

        [ForeignKey("StateTypeId")]
        public virtual StateType StateType { get; set; }

        public virtual List<ProjectTask> ProjectTasks { get; set; }

    }
}
