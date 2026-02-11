namespace TechnicalAssessment.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
