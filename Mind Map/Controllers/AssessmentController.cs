
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mind_Map.Models;
using MindMap.DTOs;
using MindMap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MindMap.Controllers
{
    [ApiController]
    [Route("api/{userId?}/[controller]")]
    public class AssessmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssessmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Level1")]
        public async Task<ActionResult<AssessmentDto>> SubmitLevel1Assessment([FromBody] AssessmentRequestDto request)
        {
            // Validate input
            if (request.Answers == null || !request.Answers.Any())
            {
                return BadRequest("At least one answer is required.");
            }

            // Validate no duplicate question IDs
            if (request.Answers.GroupBy(a => a.QuestionId).Any(g => g.Count() > 1))
            {
                return BadRequest("Duplicate question IDs are not allowed.");
            }

            // Validate all Level 1 questions (Ids 1–23) are answered
            var level1QuestionIds = await _context.Questions
                .Where(q => q.Domain.Level == "Level 1")
                .Select(q => q.Id)
                .OrderBy(q => q)
                .ToListAsync();

            if (request.Answers.Count != level1QuestionIds.Count ||
                request.Answers.Any(a => !level1QuestionIds.Contains(a.QuestionId)))
            {
                return BadRequest($"Level 1 assessment requires exactly {level1QuestionIds.Count} answers for question IDs: {string.Join(", ", level1QuestionIds)}.");
            }

            // Validate question IDs exist
            var validQuestionIds = await _context.Questions.Select(q => q.Id).ToListAsync();
            var invalidQuestionIds = request.Answers.Select(a => a.QuestionId).Except(validQuestionIds).ToList();
            if (invalidQuestionIds.Any())
            {
                return BadRequest($"Invalid question IDs: {string.Join(", ", invalidQuestionIds)}.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                user = new User
                {
                    Name = request.user.Name,
                    Age = request.user.Age,
                    Email = request.user.Email,
                    PasswordHash = request.user.PasswordHash,
                    Gender = request.user.Gender,
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var assessment = new Assessment
            {
                UserId = user.Id,
                DateTaken = DateTime.UtcNow,
                Answers = request.Answers.Select(a => new Answer
                {
                    QuestionId = a.QuestionId,
                    Score = a.Score
                }).ToList()
            };

            // Evaluate Level 1 domains
            var domainScore = await GetHighestLevel1DomainScore(assessment.Answers);
            if (domainScore == null)
            {
                return BadRequest("No valid Level 1 domain scores could be calculated.");
            }
            var domain = domainScore.Domain;
            assessment.DomainId = domain.Id;

            // Check for Level 2 recommendation
            int? recommendedLevel2DomainId = null;
            string? recommendedLevel2DomainName = null;
            if (domainScore.NeedsFurtherInquiry)
            {
                var level2Domain = await _context.Domains
                    .Where(d => d.Level == "Level 2" && d.PotentialDisorder == domain.PotentialDisorder)
                    .FirstOrDefaultAsync();
                if (level2Domain != null)
                {
                    recommendedLevel2DomainId = level2Domain.Id;
                    recommendedLevel2DomainName = level2Domain.Name;
                }
            }

            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();

            return Ok(new AssessmentDto
            {
                AssessmentId = assessment.Id,
                UserName = request.user.Name,
                UserAge = request.user.Age,
                DateTaken = assessment.DateTaken,
                DomainName = domain.Name,
                Level = domain.Level,
                Score = domainScore.Score,
                PotentialDisorder = domain.PotentialDisorder,
                Recommendation = domainScore.NeedsFurtherInquiry ? domain.Recommendation : "No significant symptoms detected.",
                RecommendedLevel2DomainId = recommendedLevel2DomainId,
                RecommendedLevel2DomainName = recommendedLevel2DomainName
            });
        }

        [HttpPost("Level2")]
        public async Task<ActionResult<AssessmentDto>> SubmitLevel2Assessment([FromBody] AssessmentRequestDto request)
        {
            // Validate input
            if (request.Answers == null || !request.Answers.Any())
            {
                return BadRequest("At least one answer is required.");
            }

            if (!request.DomainId.HasValue)
            {
                return BadRequest("DomainId is required for Level 2 assessments.");
            }

            // Validate no duplicate question IDs
            if (request.Answers.GroupBy(a => a.QuestionId).Any(g => g.Count() > 1))
            {
                return BadRequest("Duplicate question IDs are not allowed.");
            }

            // Validate question IDs exist
            var validQuestionIds = await _context.Questions.Select(q => q.Id).ToListAsync();
            var invalidQuestionIds = request.Answers.Select(a => a.QuestionId).Except(validQuestionIds).ToList();
            if (invalidQuestionIds.Any())
            {
                return BadRequest($"Invalid question IDs: {string.Join(", ", invalidQuestionIds)}.");
            }

            var domain = await _context.Domains
                .Include(d => d.Questions)
                .FirstOrDefaultAsync(d => d.Id == request.DomainId);
            if (domain == null || domain.Level != "Level 2")
            {
                return BadRequest("Invalid or non-Level 2 DomainId.");
            }

            // Validate all questions for the domain are answered
            var domainQuestionIds = domain.Questions.Select(q => q.Id).ToList();
            if (request.Answers.Count != domainQuestionIds.Count ||
                request.Answers.Any(a => !domainQuestionIds.Contains(a.QuestionId)))
            {
                return BadRequest($"Level 2 assessment for {domain.Name} requires exactly {domainQuestionIds.Count} answers for question IDs: {string.Join(", ", domainQuestionIds)}.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                user = new User
                {
                    Name = request.user.Name,
                    Age = request.user.Age,
                    Email = request.user.Email,
                    PasswordHash = request.user.PasswordHash,
                    Gender = request.user.Gender,
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var assessment = new Assessment
            {
                UserId = user.Id,
                DomainId = request.DomainId.Value,
                DateTaken = DateTime.UtcNow,
                Answers = request.Answers.Select(a => new Answer
                {
                    QuestionId = a.QuestionId,
                    Score = a.Score
                }).ToList()
            };

            var domainScore = await CalculateDomainScore(assessment.Answers, domain);

            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();

            // Generate detailed result description for Level 2
            string resultDescription = GenerateResultDescription(domain, domainScore);

            return Ok(new AssessmentDto
            {
                AssessmentId = assessment.Id,
                UserName = request.user.Name,
                UserAge = request.user.Age,
                DateTaken = assessment.DateTaken,
                DomainName = domain.Name,
                Level = domain.Level,
                Score = domainScore.Score,
                PotentialDisorder = domain.PotentialDisorder,
                Recommendation = domainScore.NeedsFurtherInquiry ? domain.Recommendation : "No significant symptoms detected.",
                ResultDescription = resultDescription
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssessmentDto>>> GetAllAssessments(int userId)
        {
            var assessments = await _context.Assessments
                .Where(a => a.UserId == userId)
                .Include(a => a.User)
                .Include(a => a.Domain)
                .Include(a => a.Answers).ThenInclude(a => a.Question)
                .ToListAsync();

            var assessmentDtos = new List<AssessmentDto>();
            foreach (var assessment in assessments)
            {
                var domainScore = await CalculateDomainScore(assessment.Answers, assessment.Domain);
                (int? recommendedLevel2DomainId, string? recommendedLevel2DomainName) = await GetRecommendedLevel2Domain(assessment.Domain, domainScore);
                string? resultDescription = assessment.Domain.Level == "Level 2" ? GenerateResultDescription(assessment.Domain, domainScore) : null;

                assessmentDtos.Add(new AssessmentDto
                {
                    AssessmentId = assessment.Id,
                    UserName = assessment.User.Name,
                    UserAge = assessment.User.Age,
                    DateTaken = assessment.DateTaken,
                    DomainName = assessment.Domain.Name,
                    Level = assessment.Domain.Level,
                    Score = domainScore.Score,
                    PotentialDisorder = assessment.Domain.PotentialDisorder,
                    Recommendation = domainScore.NeedsFurtherInquiry ? assessment.Domain.Recommendation : "No significant symptoms detected.",
                    RecommendedLevel2DomainId = recommendedLevel2DomainId,
                    RecommendedLevel2DomainName = recommendedLevel2DomainName,
                    ResultDescription = resultDescription
                });
            }

            return Ok(assessmentDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssessmentDto>> GetAssessment(int id)
        {
            var assessment = await _context.Assessments
                .Include(a => a.User)
                .Include(a => a.Domain)
                .Include(a => a.Answers).ThenInclude(a => a.Question)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessment == null)
                return NotFound("Assessment not found.");

            var domainScore = await CalculateDomainScore(assessment.Answers, assessment.Domain);
            (int? recommendedLevel2DomainId, string? recommendedLevel2DomainName) = await GetRecommendedLevel2Domain(assessment.Domain, domainScore);
            string? resultDescription = assessment.Domain.Level == "Level 2" ? GenerateResultDescription(assessment.Domain, domainScore) : null;

            return Ok(new AssessmentDto
            {
                AssessmentId = assessment.Id,
                UserName = assessment.User.Name,
                UserAge = assessment.User.Age,
                DateTaken = assessment.DateTaken,
                DomainName = assessment.Domain.Name,
                Level = assessment.Domain.Level,
                Score = domainScore.Score,
                PotentialDisorder = assessment.Domain.PotentialDisorder,
                Recommendation = domainScore.NeedsFurtherInquiry ? assessment.Domain.Recommendation : "No significant symptoms detected.",
                RecommendedLevel2DomainId = recommendedLevel2DomainId,
                RecommendedLevel2DomainName = recommendedLevel2DomainName,
                ResultDescription = resultDescription
            });
        }

        private async Task<DomainScoreDto> CalculateDomainScore(List<Answer> answers, Domain domain)
        {
            var questions = await _context.Questions
                .Where(q => q.DomainId == domain.Id)
                .ToListAsync();

            int score = 0;
            bool needsFurtherInquiry = false;

            if (domain.Level == "Level 1")
            {
                var relevantAnswers = answers
                    .Where(a => questions.Any(q => q.Id == a.QuestionId))
                    .ToList();
                score = relevantAnswers.Any() ? relevantAnswers.Max(a => a.Score) : 0;
                needsFurtherInquiry = score >= domain.Threshold;
            }
            else if (domain.Level == "Level 2")
            {
                if (domain.Name == "Anxiety")
                {
                    score = answers.Sum(a => a.Score);
                    // Convert to T-score (simplified; use actual T-score table in production)
                    score = score switch
                    {
                        <= 7 => 36,
                        <= 10 => 46,
                        <= 15 => 53,
                        <= 20 => 60,
                        <= 25 => 66,
                        <= 30 => 72,
                        _ => 82
                    };
                    needsFurtherInquiry = score >= domain.Threshold; // T-score ≥ 60
                }
                else if (domain.Name == "Mania")
                {
                    score = answers.Sum(a => a.Score);
                    needsFurtherInquiry = score >= domain.Threshold; // Score ≥ 6
                }
                else if (domain.Name == "Repetitive Thoughts and Behaviors")
                {
                    score = answers.Sum(a => a.Score);
                    needsFurtherInquiry = score >= domain.Threshold; // Score ≥ 8
                }
                else if (domain.Name == "PTSD")
                {
                    score = answers.Sum(a => a.Score);
                    needsFurtherInquiry = score >= domain.Threshold; // Score ≥ 33
                }
                else if (domain.Name == "Psychosis")
                {
                    score = answers.Any() ? answers.Max(a => a.Score) : 0;
                    needsFurtherInquiry = score >= domain.Threshold; // Score ≥ 2
                }
                else if (domain.Name == "ADHD")
                {
                    score = answers.Take(6).Count(a => a.Score >= 3); // "Often" or "Very Often"
                    needsFurtherInquiry = score >= domain.Threshold; // 4+ shaded boxes
                }
            }

            return new DomainScoreDto
            {
                Domain = domain,
                Score = score,
                NeedsFurtherInquiry = needsFurtherInquiry
            };
        }

        private async Task<DomainScoreDto?> GetHighestLevel1DomainScore(List<Answer> answers)
        {
            // Fetch all Level 1 domains and their questions
            var domains = await _context.Domains
                .Where(d => d.Level == "Level 1")
                .Include(d => d.Questions)
                .ToListAsync();

            // Calculate max score per domain
            var domainScores = domains
                .Select(d => new DomainScoreDto
                {
                    Domain = d,
                    Score = answers
                        .Where(a => d.Questions.Any(q => q.Id == a.QuestionId))
                        .Select(a => a.Score)
                        .DefaultIfEmpty(0)
                        .Max(),
                    NeedsFurtherInquiry = false
                })
                .OrderByDescending(ds => ds.Score)
                .ThenBy(ds => ds.Domain.Id) // Tiebreaker: lowest DomainId
                .ToList();

            // Set NeedsFurtherInquiry for each domain
            foreach (var ds in domainScores)
            {
                ds.NeedsFurtherInquiry = ds.Score >= ds.Domain.Threshold;
            }

            // Return the highest-scoring domain with a non-zero score
            var highestScore = domainScores.FirstOrDefault(ds => ds.Score > 0);
            return highestScore;
        }

        private async Task<(int? DomainId, string? DomainName)> GetRecommendedLevel2Domain(Domain domain, DomainScoreDto domainScore)
        {
            if (domain.Level != "Level 1" || !domainScore.NeedsFurtherInquiry)
            {
                return (null, null);
            }

            var level2Domain = await _context.Domains
                .Where(d => d.Level == "Level 2" && d.PotentialDisorder == domain.PotentialDisorder)
                .FirstOrDefaultAsync();

            return level2Domain != null
                ? (level2Domain.Id, level2Domain.Name)
                : (null, null);
        }

        private string GenerateResultDescription(Domain domain, DomainScoreDto domainScore)
        {
            if (domain.Level != "Level 2")
            {
                return string.Empty;
            }

            string severity;
            string description;

            if (!domainScore.NeedsFurtherInquiry)
            {
                severity = "No significant symptoms";
                description = $"Your score of {domainScore.Score} indicates no significant symptoms of {domain.PotentialDisorder.ToLower()}. If you have concerns, consult a healthcare professional for further evaluation.";
            }
            else
            {
                if (domain.Name == "Anxiety")
                {
                    severity = domainScore.Score switch
                    {
                        <= 53 => "Mild",
                        <= 60 => "Moderate",
                        <= 72 => "Severe",
                        _ => "Very Severe"
                    };
                    description = $"Your T-score of {domainScore.Score} indicates {severity.ToLower()} anxiety symptoms. This suggests a significant level of anxiety that may impact your daily life. {domain.Recommendation}";
                }
                else if (domain.Name == "Mania")
                {
                    severity = domainScore.Score >= 10 ? "Severe" : "Moderate";
                    description = $"Your score of {domainScore.Score} indicates {severity.ToLower()} symptoms of mania. This may suggest a manic or hypomanic episode. {domain.Recommendation}";
                }
                else if (domain.Name == "Repetitive Thoughts and Behaviors")
                {
                    severity = domainScore.Score >= 12 ? "Severe" : "Moderate";
                    description = $"Your score of {domainScore.Score} indicates {severity.ToLower()} symptoms of repetitive thoughts and behaviors, suggestive of possible obsessive-compulsive tendencies. {domain.Recommendation}";
                }
                else if (domain.Name == "PTSD")
                {
                    severity = domainScore.Score >= 50 ? "Severe" : "Moderate";
                    description = $"Your score of {domainScore.Score} indicates {severity.ToLower()} symptoms of post-traumatic stress disorder. This suggests significant trauma-related symptoms. {domain.Recommendation}";
                }
                else if (domain.Name == "Psychosis")
                {
                    severity = domainScore.Score >= 3 ? "Severe" : "Moderate";
                    description = $"Your score of {domainScore.Score} indicates {severity.ToLower()} symptoms of psychosis. This may suggest perceptual or thought disturbances. {domain.Recommendation}";
                }
                else if (domain.Name == "ADHD")
                {
                    severity = domainScore.Score >= 5 ? "Severe" : "Moderate";
                    description = $"Your score of {domainScore.Score} indicates {severity.ToLower()} symptoms of ADHD. This suggests significant inattention and/or hyperactivity. {domain.Recommendation}";
                }
                else
                {
                    severity = "Moderate";
                    description = $"Your score of {domainScore.Score} indicates potential symptoms of {domain.PotentialDisorder.ToLower()}. {domain.Recommendation}";
                }
            }

            return $"Result: {severity}. {description}";
        }
    }
}
