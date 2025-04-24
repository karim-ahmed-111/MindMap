namespace Mind_Map.DTOs
{
    public class AssessmentResponseDto
    {
        internal string ResultDescription;
        internal string Level;

        public string DomainName { get; set; }
        public int Score { get; set; }
        public string PotentialDisorder { get; set; }
        public string Recommendation { get; set; }
    }
}
