using System.ComponentModel.DataAnnotations;

namespace OmnIDEApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public required string Language { get; set; }
        [Required]
        public required string Status { get; set; }
    }
}