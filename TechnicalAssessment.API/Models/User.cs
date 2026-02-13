using System.ComponentModel.DataAnnotations;

namespace TechnicalAssessment.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(150)]
        public required string Email { get; set; }

        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
