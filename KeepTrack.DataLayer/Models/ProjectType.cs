using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KeepTrack.DataLayer.Models
{
    public class ProjectType
    {
        protected ProjectType()
        {

        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string DataName { get; set; }

        public string Image { get; set; }

        //Relations

        public virtual List<Project> Projects { get; set; }
    }
}
