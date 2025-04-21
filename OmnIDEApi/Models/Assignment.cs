using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmnIDEApi.Models
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AssignmentID { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }

        public bool Success { get; set; }

        public virtual Student Student { get; set; }
    }
}