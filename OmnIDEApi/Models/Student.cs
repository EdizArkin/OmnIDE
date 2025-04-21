using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmnIDEApi.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Surname { get; set; }

        // Navigation property
        public required ICollection<Assignment> Assignments { get; set; }
    }
}
