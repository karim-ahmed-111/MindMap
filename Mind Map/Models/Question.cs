namespace MindMap.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int DomainId { get; set; }
        public Domain Domain { get; set; }
        public List<ScoringOption> ScoringOptions { get; set; } = new List<ScoringOption>(); // Added to store scoring options
    }
}
