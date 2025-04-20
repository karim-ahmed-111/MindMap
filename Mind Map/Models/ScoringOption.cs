namespace MindMap.Models
{
    public class ScoringOption // New model for storing scoring options
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int Score { get; set; }
        public string Description { get; set; }
        public Question Question { get; set; }
    }
}
