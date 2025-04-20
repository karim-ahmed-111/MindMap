namespace MindMap.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int AssessmentId { get; set; }
        public int QuestionId { get; set; }
        public int Score { get; set; }
        public Assessment Assessment { get; set; }
        public Question Question { get; set; }
    }
}
