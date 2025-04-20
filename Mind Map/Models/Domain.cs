namespace MindMap.Models
{
    public class Domain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PotentialDisorder { get; set; }
        public int Threshold { get; set; }
        public string Recommendation { get; set; }
        public string Level { get; set; } // Added to distinguish Level 1 vs Level 2
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
