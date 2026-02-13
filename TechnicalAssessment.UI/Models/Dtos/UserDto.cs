using System.Collections.Generic;

namespace TechnicalAssessment.UI.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<GroupDto> Groups { get; set; } = new();
    }
}
