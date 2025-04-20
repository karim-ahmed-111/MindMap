using MindMap.Models;

namespace MindMap.DTOs
{
    public class DomainScoreDto
    {
        public Domain Domain { get; set; }
        public int Score { get; set; }
        public bool NeedsFurtherInquiry { get; set; }
    }
}
