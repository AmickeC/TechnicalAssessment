using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechnicalAssessment.UI.Models
{
    public class UserCreateModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public List<int> GroupIds { get; set; } = new();
    }
}
