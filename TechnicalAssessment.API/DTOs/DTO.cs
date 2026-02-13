namespace TechnicalAssessment.DTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new();
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<GroupDto> Groups { get; set; } = new();
    }

    public class UserCreateDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<int> GroupIds { get; set; } = new(); 
    }

    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<int>? GroupIds { get; set; } 
    }
}
