using System.Collections.Generic;

namespace TechnicalAssessment.UI.Models
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public List<PermissionDto> Permissions { get; set; } = new();
        public int UserCount { get; set; }
    }
}
