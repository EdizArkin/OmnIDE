using System.ComponentModel.DataAnnotations;

namespace OmnIDEApi.Models
{
    public class ProjectConfiguration
    {
        [Key]
        public int Id { get; set; }
        public string ProgrammingLanguage { get; set; }
        public string ProjectPath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}