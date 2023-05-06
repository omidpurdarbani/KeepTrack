using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KeepTrack.DataLayer.Models
{
    public class StateType
    {
        protected StateType()
        {

        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        //Relations

        public virtual List<Project> Projects { get; set; }

        public virtual List<ProjectTask> ProjectTasks { get; set; }
    }
}
