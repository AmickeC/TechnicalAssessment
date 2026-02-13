using System.ComponentModel.DataAnnotations;

namespace TechnicalAssessment.Models
{
    public class Permission
    {
        public int Id { get; set; }

         [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
