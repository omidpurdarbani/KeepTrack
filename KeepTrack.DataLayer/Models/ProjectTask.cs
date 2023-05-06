using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeepTrack.DataLayer.Models
{
    public class ProjectTask
    {
        public ProjectTask()
        {

        }
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public int ProjectId { get; set; }

        public int? StateTypeId { get; set; }

        //Relations

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("StateTypeId")]
        public virtual StateType StateType { get; set; }

    }
}
