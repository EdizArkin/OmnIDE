using System.ComponentModel.DataAnnotations;

namespace OmnIDEApi.Models
{
    public class ProjectConfiguration
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string ProgrammingLanguage { get; set; }
        [Required]
        public required string ProjectPath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}