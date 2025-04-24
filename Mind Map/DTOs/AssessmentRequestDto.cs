using Mind_Map.Models;

namespace MindMap.DTOs
{
    public class AssessmentRequestDto
    {
        public int UserId { get; set; }
        public int? DomainId { get; set; }
        public List<AnswerRequestDto> Answers { get; set; } = new List<AnswerRequestDto>();
    }
}
