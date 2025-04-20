using MindMap.Models;

namespace MindMap.DTOs
{
    public class AssessmentDto
    {
        public int AssessmentId { get; set; }
        public string UserName { get; set; }
        public int UserAge { get; set; }
        public DateTime DateTaken { get; set; }
        public string DomainName { get; set; }
        public string Level { get; set; }
        public int Score { get; set; }
        public string PotentialDisorder { get; set; }
        public string Recommendation { get; set; }
        public int? RecommendedLevel2DomainId { get; set; }
        public string? RecommendedLevel2DomainName { get; set; }
        public string? ResultDescription { get; set; } // Added for Level 2
    }
}
