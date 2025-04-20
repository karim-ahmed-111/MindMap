namespace Mind_Map.Models
{
    using Microsoft.EntityFrameworkCore;
    using MindMap.Models;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<PersonalityTrait> PersonalityTraits { get; set; }
        public DbSet<PersonalityTestAns> PersonalityTestAnswers { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<ScoringOption> ScoringOptions { get; set; } // Added for scoring options

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationships
            modelBuilder.Entity<Assessment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Assessments)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Assessment>()
                .HasOne(a => a.Domain)
                .WithMany()
                .HasForeignKey(a => a.DomainId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Assessment)
                .WithMany(a => a.Answers)
                .HasForeignKey(a => a.AssessmentId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Domain)
                .WithMany(d => d.Questions)
                .HasForeignKey(q => q.DomainId);

            modelBuilder.Entity<ScoringOption>()
                .HasOne(so => so.Question)
                .WithMany(q => q.ScoringOptions)
                .HasForeignKey(so => so.QuestionId);

            // Seed Domains (Level 1 and Level 2)
            modelBuilder.Entity<Domain>().HasData(
                // Level 1 Domains (existing)
                new Domain { Id = 1, Name = "Depression", PotentialDisorder = "Depressive Disorders", Threshold = 2, Recommendation = "Your responses suggest possible depression. Consider the PROMIS Emotional Distress—Depression—Short Form and consult a mental health professional.", Level = "Level 1" },
                new Domain { Id = 2, Name = "Anger", PotentialDisorder = "Anger-Related Issues", Threshold = 2, Recommendation = "Your responses indicate potential anger issues. Consider the PROMIS Emotional Distress—Anger—Short Form and consult a mental health professional.", Level = "Level 1" },
                new Domain { Id = 3, Name = "Mania", PotentialDisorder = "Bipolar Disorder or Manic Episodes", Threshold = 2, Recommendation = "Your responses suggest possible manic symptoms. Consider the Altman Self-Rating Mania Scale and consult a mental health professional.", Level = "Level 1" },
                new Domain { Id = 4, Name = "Anxiety", PotentialDisorder = "Anxiety Disorders", Threshold = 2, Recommendation = "Your responses indicate possible anxiety. Consider the PROMIS Emotional Distress—Anxiety—Short Form and consult a mental health professional.", Level = "Level 1" },
                new Domain { Id = 5, Name = "Somatic Symptoms", PotentialDisorder = "Somatic Symptom Disorders", Threshold = 2, Recommendation = "Your responses suggest physical symptoms that may be stress-related. Consider the Patient Health Questionnaire 15 (PHQ-15) and consult a medical professional.", Level = "Level 1" },
                new Domain { Id = 6, Name = "Suicidal Ideation", PotentialDisorder = "Suicidal Ideation", Threshold = 1, Recommendation = "Urgent: Your responses indicate thoughts of self-harm. Please contact a mental health professional or crisis hotline immediately.", Level = "Level 1" },
                new Domain { Id = 7, Name = "Psychosis", PotentialDisorder = "Psychotic Disorders", Threshold = 1, Recommendation = "Urgent: Your responses suggest possible psychotic symptoms. Please consult a mental health professional immediately.", Level = "Level 1" },
                new Domain { Id = 8, Name = "Sleep Problems", PotentialDisorder = "Sleep Disorders", Threshold = 2, Recommendation = "Your responses indicate sleep difficulties. Consider the PROMIS—Sleep Disturbance—Short Form and consult a sleep specialist or mental health professional.", Level = "Level 1" },
                new Domain { Id = 9, Name = "Memory", PotentialDisorder = "Cognitive Impairment", Threshold = 2, Recommendation = "Your responses suggest memory issues. Consult a neurologist or mental health professional for a cognitive assessment.", Level = "Level 1" },
                new Domain { Id = 10, Name = "Repetitive Thoughts and Behaviors", PotentialDisorder = "Obsessive-Compulsive Disorder (OCD)", Threshold = 2, Recommendation = "Your responses suggest possible OCD. Consider the Florida Obsessive-Compulsive Inventory (FOCI) Severity Scale and consult a mental health professional.", Level = "Level 1" },
                new Domain { Id = 11, Name = "Dissociation", PotentialDisorder = "Dissociative Disorders", Threshold = 2, Recommendation = "Your responses indicate possible dissociation. Consult a mental health professional for a specialized assessment.", Level = "Level 1" },
                new Domain { Id = 12, Name = "Personality Functioning", PotentialDisorder = "Personality Disorders", Threshold = 2, Recommendation = "Your responses suggest challenges with identity or relationships. Consult a mental health professional for a personality assessment.", Level = "Level 1" },
                new Domain { Id = 13, Name = "Substance Use", PotentialDisorder = "Substance Use Disorders", Threshold = 1, Recommendation = "Your responses indicate possible substance misuse. Consider the NIDA-modified ASSIST and consult a substance abuse specialist.", Level = "Level 1" },
                // Level 2 Domains (new)
                new Domain { Id = 14, Name = "Anxiety", PotentialDisorder = "Anxiety Disorders", Threshold = 60, Recommendation = "Your responses indicate significant anxiety symptoms. Consult a mental health professional for further evaluation.", Level = "Level 2" }, // T-score ≥ 60 (Moderate)
                new Domain { Id = 15, Name = "Mania", PotentialDisorder = "Bipolar Disorder or Manic Episodes", Threshold = 6, Recommendation = "Your responses suggest a high probability of manic symptoms. Consult a mental health professional immediately.", Level = "Level 2" }, // Score ≥ 6
                new Domain { Id = 16, Name = "Repetitive Thoughts and Behaviors", PotentialDisorder = "Obsessive-Compulsive Disorder (OCD)", Threshold = 8, Recommendation = "Your responses suggest significant OCD symptoms. Consult a mental health professional for further evaluation.", Level = "Level 2" }, // Score ≥ 8
                new Domain { Id = 17, Name = "PTSD", PotentialDisorder = "Post-Traumatic Stress Disorder", Threshold = 33, Recommendation = "Your responses indicate significant PTSD symptoms. Consult a mental health professional for further evaluation.", Level = "Level 2" }, // Score ≥ 33 (suggested cutoff)
                new Domain { Id = 18, Name = "Psychosis", PotentialDisorder = "Psychotic Disorders", Threshold = 2, Recommendation = "Your responses suggest possible psychotic symptoms. Consult a mental health professional immediately.", Level = "Level 2" }, // Score ≥ 2 (mild or greater)
                new Domain { Id = 19, Name = "ADHD", PotentialDisorder = "Attention-Deficit/Hyperactivity Disorder", Threshold = 4, Recommendation = "Your responses suggest possible ADHD symptoms. Consult a mental health professional for further evaluation.", Level = "Level 2" } // 4+ shaded boxes in Part A
            );

            // Seed Questions (Level 1 and Level 2)
            int questionId = 1;
            int scoringOptionId = 1;

            // Level 1 Questions (existing)
            modelBuilder.Entity<Question>().HasData(
                new Question { Id = questionId++, Text = "Little interest or pleasure in doing things?", DomainId = 1 },
                new Question { Id = questionId++, Text = "Feeling down, depressed, or hopeless?", DomainId = 1 },
                new Question { Id = questionId++, Text = "Feeling more irritated, grouchy, or angry than usual?", DomainId = 2 },
                new Question { Id = questionId++, Text = "Sleeping less than usual, but still have a lot of energy?", DomainId = 3 },
                new Question { Id = questionId++, Text = "Starting lots more projects than usual or doing more risky things than usual?", DomainId = 3 },
                new Question { Id = questionId++, Text = "Feeling nervous, anxious, frightened, worried, or on edge?", DomainId = 4 },
                new Question { Id = questionId++, Text = "Feeling panic or being frightened?", DomainId = 4 },
                new Question { Id = questionId++, Text = "Avoiding situations that make you anxious?", DomainId = 4 },
                new Question { Id = questionId++, Text = "Unexplained aches and pains (e.g., head, back, joints, abdomen, legs)?", DomainId = 5 },
                new Question { Id = questionId++, Text = "Feeling that your illnesses are not being taken seriously enough?", DomainId = 5 },
                new Question { Id = questionId++, Text = "Thoughts of actually hurting yourself?", DomainId = 6 },
                new Question { Id = questionId++, Text = "Hearing things other people couldn’t hear, such as voices even when no one was around?", DomainId = 7 },
                new Question { Id = questionId++, Text = "Feeling that someone could hear your thoughts, or that you could hear what another person was thinking?", DomainId = 7 },
                new Question { Id = questionId++, Text = "Problems with sleep that affected your sleep quality over all?", DomainId = 8 },
                new Question { Id = questionId++, Text = "Problems with memory (e.g., learning new information) or with location (e.g., finding your way home)?", DomainId = 9 },
                new Question { Id = questionId++, Text = "Unpleasant thoughts, urges, or images that repeatedly enter your mind?", DomainId = 10 },
                new Question { Id = questionId++, Text = "Feeling driven to perform certain behaviors or mental acts over and over again?", DomainId = 10 },
                new Question { Id = questionId++, Text = "Feeling detached or distant from yourself, your body, your physical surroundings, or your memories?", DomainId = 11 },
                new Question { Id = questionId++, Text = "Not knowing who you really are or what you want out of life?", DomainId = 12 },
                new Question { Id = questionId++, Text = "Not feeling close to other people or enjoying your relationships with them?", DomainId = 12 },
                new Question { Id = questionId++, Text = "Drinking at least 4 drinks of any kind of alcohol in a single day?", DomainId = 13 },
                new Question { Id = questionId++, Text = "Smoking any cigarettes, a cigar, or pipe, or using snuff or chewing tobacco?", DomainId = 13 },
                new Question { Id = questionId++, Text = "Using any of the following medicines ON YOUR OWN, that is, without a doctor’s prescription, in greater amounts or longer than prescribed [e.g., painkillers (like Vicodin), stimulants (like Ritalin or Adderall), sedatives or tranquilizers (like sleeping pills or Valium), or drugs like marijuana, cocaine or crack, club drugs (like ecstasy), hallucinogens (like LSD), heroin, inhalants or solvents (like glue), or methamphetamine (like speed)]?", DomainId = 13 }
            );

            // Scoring Options for Level 1 Questions
            var level1ScoringOptions = new[]
            {
                new { Score = 0, Description = "None/Not at all" },
                new { Score = 1, Description = "Slight/Rare, less than a day or two" },
                new { Score = 2, Description = "Mild/Several days" },
                new { Score = 3, Description = "Moderate/More than half the days" },
                new { Score = 4, Description = "Severe/Nearly every day" }
            };
            for (int i = 1; i <= 23; i++)
            {
                foreach (var option in level1ScoringOptions)
                {
                    modelBuilder.Entity<ScoringOption>().HasData(
                        new ScoringOption { Id = scoringOptionId++, QuestionId = i, Score = option.Score, Description = option.Description }
                    );
                }
            }

            // Level 2 Anxiety Questions
            var anxietyQuestions = new[]
            {
                "I felt fearful.",
                "I felt anxious.",
                "I felt worried.",
                "I found it hard to focus on anything other than my anxiety.",
                "I felt nervous.",
                "I felt uneasy.",
                "I felt tense."
            };
            var anxietyScoringOptions = new[]
            {
                new { Score = 1, Description = "Never" },
                new { Score = 2, Description = "Rarely" },
                new { Score = 3, Description = "Sometimes" },
                new { Score = 4, Description = "Often" },
                new { Score = 5, Description = "Always" }
            };
            foreach (var question in anxietyQuestions)
            {
                modelBuilder.Entity<Question>().HasData(
                    new Question { Id = questionId, Text = question, DomainId = 14 }
                );
                foreach (var option in anxietyScoringOptions)
                {
                    modelBuilder.Entity<ScoringOption>().HasData(
                        new ScoringOption { Id = scoringOptionId++, QuestionId = questionId, Score = option.Score, Description = option.Description }
                    );
                }
                questionId++;
            }

            // Level 2 Mania Questions
            var maniaQuestions = new[]
            {
                "Feel happier or more cheerful than usual.",
                "Feel more self-confident than usual.",
                "Need less sleep than usual.",
                "Talk more than usual.",
                "Been more active (either socially, sexually, at work, home, or school) than usual."
            };
            var maniaScoringOptions = new[]
            {
                new { Score = 0, Description = "Not at all" },
                new { Score = 1, Description = "Occasionally" },
                new { Score = 2, Description = "Often" },
                new { Score = 3, Description = "Frequently" },
                new { Score = 4, Description = "All the time" }
            };
            foreach (var question in maniaQuestions)
            {
                modelBuilder.Entity<Question>().HasData(
                    new Question { Id = questionId, Text = question, DomainId = 15 }
                );
                foreach (var option in maniaScoringOptions)
                {
                    modelBuilder.Entity<ScoringOption>().HasData(
                        new ScoringOption { Id = scoringOptionId++, QuestionId = questionId, Score = option.Score, Description = option.Description }
                    );
                }
                questionId++;
            }

            // Level 2 OCD Questions
            var ocdQuestions = new[]
            {
                "On average, how much time is occupied by these thoughts or behaviors each day?",
                "How much distress do these thoughts or behaviors cause you?",
                "How hard is it for you to control these thoughts or behaviors?",
                "How much do these thoughts or behaviors cause you to avoid doing anything, going anyplace, or being with anyone?",
                "How much do these thoughts or behaviors interfere with school, work, or your social or family life?"
            };
            var ocdScoringOptions = new[]
            {
                new { Score = 0, Description = "None" },
                new { Score = 1, Description = "Mild" },
                new { Score = 2, Description = "Moderate" },
                new { Score = 3, Description = "Severe" },
                new { Score = 4, Description = "Extreme" }
            };
            foreach (var question in ocdQuestions)
            {
                modelBuilder.Entity<Question>().HasData(
                    new Question { Id = questionId, Text = question, DomainId = 16 }
                );
                foreach (var option in ocdScoringOptions)
                {
                    modelBuilder.Entity<ScoringOption>().HasData(
                        new ScoringOption { Id = scoringOptionId++, QuestionId = questionId, Score = option.Score, Description = option.Description }
                    );
                }
                questionId++;
            }

            // Level 2 PTSD Questions
            var ptsdQuestions = new[]
            {
                "Repeated, disturbing, and unwanted memories of the stressful experience?",
                "Repeated, disturbing dreams of the stressful experience?",
                "Suddenly feeling or acting as if the stressful experience were actually happening again (as if you were actually back there reliving it)?",
                "Feeling very upset when something reminded you of the stressful experience?",
                "Having strong physical reactions when something reminded you of the stressful experience (for example, heart pounding, trouble breathing, sweating)?",
                "Avoiding memories, thoughts, or feelings related to the stressful experience?",
                "Avoiding external reminders of the stressful experience (for example, people, places, conversations, activities, objects, or situations)?",
                "Trouble remembering important parts of the stressful experience?",
                "Having strong negative beliefs about yourself, other people, or the world (for example, having thoughts such as: I am bad, there is something seriously wrong with me, no one can be trusted, the world is completely dangerous)?",
                "Blaming yourself or someone else for the stressful experience or what happened after it?",
                "Having strong negative feelings such as fear, horror, anger, guilt, or shame?",
                "Loss of interest in activities that you used to enjoy?",
                "Feeling distant or cut off from other people?",
                "Trouble experiencing positive feelings (for example, being unable to feel happiness or have loving feelings for people close to you)?",
                "Irritable behavior, angry outbursts, or acting aggressively?",
                "Taking too many risks or doing things that could cause you harm?",
                "Being 'superalert' or watchful or on guard?",
                "Feeling jumpy or easily startled?",
                "Having difficulty concentrating?",
                "Trouble falling or staying asleep?"
            };
            var ptsdScoringOptions = new[]
            {
                new { Score = 0, Description = "Not at all" },
                new { Score = 1, Description = "A little bit" },
                new { Score = 2, Description = "Moderately" },
                new { Score = 3, Description = "Quite a bit" },
                new { Score = 4, Description = "Extremely" }
            };
            foreach (var question in ptsdQuestions)
            {
                modelBuilder.Entity<Question>().HasData(
                    new Question { Id = questionId, Text = question, DomainId = 17 }
                );
                foreach (var option in ptsdScoringOptions)
                {
                    modelBuilder.Entity<ScoringOption>().HasData(
                        new ScoringOption { Id = scoringOptionId++, QuestionId = questionId, Score = option.Score, Description = option.Description }
                    );
                }
                questionId++;
            }

            // Level 2 Psychosis Questions
            var psychosisQuestions = new[]
            {
                "Hallucinations",
                "Delusions",
                "Disorganized speech",
                "Abnormal psychomotor behavior",
                "Negative symptoms (restricted emotional expression or avolition)",
                "Impaired cognition",
                "Depression",
                "Mania"
            };
            var psychosisScoringOptions = new[]
            {
                new { Score = 0, Description = "Not present" },
                new { Score = 1, Description = "Equivocal" },
                new { Score = 2, Description = "Present, but mild" },
                new { Score = 3, Description = "Present and moderate" },
                new { Score = 4, Description = "Present and severe" }
            };
            foreach (var question in psychosisQuestions)
            {
                modelBuilder.Entity<Question>().HasData(
                    new Question { Id = questionId, Text = question, DomainId = 18 }
                );
                foreach (var option in psychosisScoringOptions)
                {
                    modelBuilder.Entity<ScoringOption>().HasData(
                        new ScoringOption { Id = scoringOptionId++, QuestionId = questionId, Score = option.Score, Description = option.Description }
                    );
                }
                questionId++;
            }

            // Level 2 ADHD Questions
            var adhdQuestions = new[]
            {
                "How often do you have trouble wrapping up the final details of a project, once the challenging parts have been done?",
                "How often do you have difficulty getting things in order when you have to do a task that requires organization?",
                "How often do you have problems remembering appointments or obligations?",
                "When you have a task that requires a lot of thought, how often do you avoid or delay getting started?",
                "How often do you fidget or squirm with your hands or feet when you have to sit down for a long time?",
                "How often do you feel overly active and compelled to do things, like you were driven by a motor?",
                "How often do you make careless mistakes when you have to work on a boring or difficult project?",
                "How often do you have difficulty keeping your attention when you are doing boring or repetitive work?",
                "How often do you have difficulty concentrating on what people say to you, even when they are speaking to you directly?",
                "How often do you misplace or have difficulty finding things at home or at work?",
                "How often are you distracted by activity or noise around you?",
                "How often do you leave your seat in meetings or other situations in which you are expected to remain seated?",
                "How often do you feel restless or fidgety?",
                "How often do you have difficulty unwinding and relaxing when you have time to yourself?",
                "How often do you find yourself talking too much when you are in social situations?",
                "When you're in a conversation, how often do you find yourself finishing the sentences of the people you are talking to, before they can finish them themselves?",
                "How often do you have difficulty waiting your turn in situations when turn taking is required?",
                "How often do you interrupt others when they are busy?"
            };
            var adhdScoringOptions = new[]
            {
                new { Score = 0, Description = "Never" },
                new { Score = 1, Description = "Rarely" },
                new { Score = 2, Description = "Sometimes" },
                new { Score = 3, Description = "Often" },
                new { Score = 4, Description = "Very Often" }
            };
            foreach (var question in adhdQuestions)
            {
                modelBuilder.Entity<Question>().HasData(
                    new Question { Id = questionId, Text = question, DomainId = 19 }
                );
                foreach (var option in adhdScoringOptions)
                {
                    modelBuilder.Entity<ScoringOption>().HasData(
                        new ScoringOption { Id = scoringOptionId++, QuestionId = questionId, Score = option.Score, Description = option.Description }
                    );
                }
                questionId++;
            }


            // Optional: Seed initial personality traits
            modelBuilder.Entity<PersonalityTrait>().HasData(
                    new PersonalityTrait { Id = 1, Name = "Introversion" },
                    new PersonalityTrait { Id = 2, Name = "Extraversion" },
                    new PersonalityTrait { Id = 3, Name = "Thinking" },
                    new PersonalityTrait { Id = 4, Name = "Feeling" }
                );
        }
    }
}
