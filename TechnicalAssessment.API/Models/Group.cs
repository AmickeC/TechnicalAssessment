using System.ComponentModel.DataAnnotations;

namespace TechnicalAssessment.Models
{
    public class Group
    {
        public int Id { get; set; }
        
        [MaxLength(100)]  
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
