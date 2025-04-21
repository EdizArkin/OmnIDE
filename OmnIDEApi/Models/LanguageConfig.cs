using System.ComponentModel.DataAnnotations;

namespace OmnIDEApi.Models
{
    public class LanguageConfig
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string LanguageName { get; set; }

        [Required]
        public string CompilerPath { get; set; }

        public string IDESettings { get; set; }
    }
}