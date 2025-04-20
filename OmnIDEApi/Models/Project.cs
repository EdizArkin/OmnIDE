namespace OmnIDEApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Language { get; set; }
        public string Status { get; set; }
    }
}