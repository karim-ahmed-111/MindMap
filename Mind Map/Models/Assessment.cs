using Mind_Map.Models;

namespace MindMap.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTaken { get; set; }
        public User User { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public int DomainId { get; set; } // Added to link assessment to a specific domain
        public Domain Domain { get; set; }
    }
}
