using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mind_Map.Migrations
{
    /// <inheritdoc />
    public partial class DisorderTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PotentialDisorder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Threshold = table.Column<int>(type: "int", nullable: false),
                    Recommendation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalityTraits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalityTraits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PersonalityType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalityType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalityTestAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TraitId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalityTestAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalityTestAnswers_PersonalityTraits_TraitId",
                        column: x => x.TraitId,
                        principalTable: "PersonalityTraits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateTaken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DomainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_Domains_DomainId",
                        column: x => x.DomainId,
                        principalTable: "Domains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assessments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScoringOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Domains",
                columns: new[] { "Id", "Level", "Name", "PotentialDisorder", "Recommendation", "Threshold" },
                values: new object[,]
                {
                    { 1, "Level 1", "Depression", "Depressive Disorders", "Your responses suggest possible depression. Consider the PROMIS Emotional Distress—Depression—Short Form and consult a mental health professional.", 2 },
                    { 2, "Level 1", "Anger", "Anger-Related Issues", "Your responses indicate potential anger issues. Consider the PROMIS Emotional Distress—Anger—Short Form and consult a mental health professional.", 2 },
                    { 3, "Level 1", "Mania", "Bipolar Disorder or Manic Episodes", "Your responses suggest possible manic symptoms. Consider the Altman Self-Rating Mania Scale and consult a mental health professional.", 2 },
                    { 4, "Level 1", "Anxiety", "Anxiety Disorders", "Your responses indicate possible anxiety. Consider the PROMIS Emotional Distress—Anxiety—Short Form and consult a mental health professional.", 2 },
                    { 5, "Level 1", "Somatic Symptoms", "Somatic Symptom Disorders", "Your responses suggest physical symptoms that may be stress-related. Consider the Patient Health Questionnaire 15 (PHQ-15) and consult a medical professional.", 2 },
                    { 6, "Level 1", "Suicidal Ideation", "Suicidal Ideation", "Urgent: Your responses indicate thoughts of self-harm. Please contact a mental health professional or crisis hotline immediately.", 1 },
                    { 7, "Level 1", "Psychosis", "Psychotic Disorders", "Urgent: Your responses suggest possible psychotic symptoms. Please consult a mental health professional immediately.", 1 },
                    { 8, "Level 1", "Sleep Problems", "Sleep Disorders", "Your responses indicate sleep difficulties. Consider the PROMIS—Sleep Disturbance—Short Form and consult a sleep specialist or mental health professional.", 2 },
                    { 9, "Level 1", "Memory", "Cognitive Impairment", "Your responses suggest memory issues. Consult a neurologist or mental health professional for a cognitive assessment.", 2 },
                    { 10, "Level 1", "Repetitive Thoughts and Behaviors", "Obsessive-Compulsive Disorder (OCD)", "Your responses suggest possible OCD. Consider the Florida Obsessive-Compulsive Inventory (FOCI) Severity Scale and consult a mental health professional.", 2 },
                    { 11, "Level 1", "Dissociation", "Dissociative Disorders", "Your responses indicate possible dissociation. Consult a mental health professional for a specialized assessment.", 2 },
                    { 12, "Level 1", "Personality Functioning", "Personality Disorders", "Your responses suggest challenges with identity or relationships. Consult a mental health professional for a personality assessment.", 2 },
                    { 13, "Level 1", "Substance Use", "Substance Use Disorders", "Your responses indicate possible substance misuse. Consider the NIDA-modified ASSIST and consult a substance abuse specialist.", 1 },
                    { 14, "Level 2", "Anxiety", "Anxiety Disorders", "Your responses indicate significant anxiety symptoms. Consult a mental health professional for further evaluation.", 60 },
                    { 15, "Level 2", "Mania", "Bipolar Disorder or Manic Episodes", "Your responses suggest a high probability of manic symptoms. Consult a mental health professional immediately.", 6 },
                    { 16, "Level 2", "Repetitive Thoughts and Behaviors", "Obsessive-Compulsive Disorder (OCD)", "Your responses suggest significant OCD symptoms. Consult a mental health professional for further evaluation.", 8 },
                    { 17, "Level 2", "PTSD", "Post-Traumatic Stress Disorder", "Your responses indicate significant PTSD symptoms. Consult a mental health professional for further evaluation.", 33 },
                    { 18, "Level 2", "Psychosis", "Psychotic Disorders", "Your responses suggest possible psychotic symptoms. Consult a mental health professional immediately.", 2 },
                    { 19, "Level 2", "ADHD", "Attention-Deficit/Hyperactivity Disorder", "Your responses suggest possible ADHD symptoms. Consult a mental health professional for further evaluation.", 4 }
                });

            migrationBuilder.InsertData(
                table: "PersonalityTraits",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Introversion" },
                    { 2, "Extraversion" },
                    { 3, "Thinking" },
                    { 4, "Feeling" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "DomainId", "Text" },
                values: new object[,]
                {
                    { 1, 1, "Little interest or pleasure in doing things?" },
                    { 2, 1, "Feeling down, depressed, or hopeless?" },
                    { 3, 2, "Feeling more irritated, grouchy, or angry than usual?" },
                    { 4, 3, "Sleeping less than usual, but still have a lot of energy?" },
                    { 5, 3, "Starting lots more projects than usual or doing more risky things than usual?" },
                    { 6, 4, "Feeling nervous, anxious, frightened, worried, or on edge?" },
                    { 7, 4, "Feeling panic or being frightened?" },
                    { 8, 4, "Avoiding situations that make you anxious?" },
                    { 9, 5, "Unexplained aches and pains (e.g., head, back, joints, abdomen, legs)?" },
                    { 10, 5, "Feeling that your illnesses are not being taken seriously enough?" },
                    { 11, 6, "Thoughts of actually hurting yourself?" },
                    { 12, 7, "Hearing things other people couldn’t hear, such as voices even when no one was around?" },
                    { 13, 7, "Feeling that someone could hear your thoughts, or that you could hear what another person was thinking?" },
                    { 14, 8, "Problems with sleep that affected your sleep quality over all?" },
                    { 15, 9, "Problems with memory (e.g., learning new information) or with location (e.g., finding your way home)?" },
                    { 16, 10, "Unpleasant thoughts, urges, or images that repeatedly enter your mind?" },
                    { 17, 10, "Feeling driven to perform certain behaviors or mental acts over and over again?" },
                    { 18, 11, "Feeling detached or distant from yourself, your body, your physical surroundings, or your memories?" },
                    { 19, 12, "Not knowing who you really are or what you want out of life?" },
                    { 20, 12, "Not feeling close to other people or enjoying your relationships with them?" },
                    { 21, 13, "Drinking at least 4 drinks of any kind of alcohol in a single day?" },
                    { 22, 13, "Smoking any cigarettes, a cigar, or pipe, or using snuff or chewing tobacco?" },
                    { 23, 13, "Using any of the following medicines ON YOUR OWN, that is, without a doctor’s prescription, in greater amounts or longer than prescribed [e.g., painkillers (like Vicodin), stimulants (like Ritalin or Adderall), sedatives or tranquilizers (like sleeping pills or Valium), or drugs like marijuana, cocaine or crack, club drugs (like ecstasy), hallucinogens (like LSD), heroin, inhalants or solvents (like glue), or methamphetamine (like speed)]?" },
                    { 24, 14, "I felt fearful." },
                    { 25, 14, "I felt anxious." },
                    { 26, 14, "I felt worried." },
                    { 27, 14, "I found it hard to focus on anything other than my anxiety." },
                    { 28, 14, "I felt nervous." },
                    { 29, 14, "I felt uneasy." },
                    { 30, 14, "I felt tense." },
                    { 31, 15, "Feel happier or more cheerful than usual." },
                    { 32, 15, "Feel more self-confident than usual." },
                    { 33, 15, "Need less sleep than usual." },
                    { 34, 15, "Talk more than usual." },
                    { 35, 15, "Been more active (either socially, sexually, at work, home, or school) than usual." },
                    { 36, 16, "On average, how much time is occupied by these thoughts or behaviors each day?" },
                    { 37, 16, "How much distress do these thoughts or behaviors cause you?" },
                    { 38, 16, "How hard is it for you to control these thoughts or behaviors?" },
                    { 39, 16, "How much do these thoughts or behaviors cause you to avoid doing anything, going anyplace, or being with anyone?" },
                    { 40, 16, "How much do these thoughts or behaviors interfere with school, work, or your social or family life?" },
                    { 41, 17, "Repeated, disturbing, and unwanted memories of the stressful experience?" },
                    { 42, 17, "Repeated, disturbing dreams of the stressful experience?" },
                    { 43, 17, "Suddenly feeling or acting as if the stressful experience were actually happening again (as if you were actually back there reliving it)?" },
                    { 44, 17, "Feeling very upset when something reminded you of the stressful experience?" },
                    { 45, 17, "Having strong physical reactions when something reminded you of the stressful experience (for example, heart pounding, trouble breathing, sweating)?" },
                    { 46, 17, "Avoiding memories, thoughts, or feelings related to the stressful experience?" },
                    { 47, 17, "Avoiding external reminders of the stressful experience (for example, people, places, conversations, activities, objects, or situations)?" },
                    { 48, 17, "Trouble remembering important parts of the stressful experience?" },
                    { 49, 17, "Having strong negative beliefs about yourself, other people, or the world (for example, having thoughts such as: I am bad, there is something seriously wrong with me, no one can be trusted, the world is completely dangerous)?" },
                    { 50, 17, "Blaming yourself or someone else for the stressful experience or what happened after it?" },
                    { 51, 17, "Having strong negative feelings such as fear, horror, anger, guilt, or shame?" },
                    { 52, 17, "Loss of interest in activities that you used to enjoy?" },
                    { 53, 17, "Feeling distant or cut off from other people?" },
                    { 54, 17, "Trouble experiencing positive feelings (for example, being unable to feel happiness or have loving feelings for people close to you)?" },
                    { 55, 17, "Irritable behavior, angry outbursts, or acting aggressively?" },
                    { 56, 17, "Taking too many risks or doing things that could cause you harm?" },
                    { 57, 17, "Being 'superalert' or watchful or on guard?" },
                    { 58, 17, "Feeling jumpy or easily startled?" },
                    { 59, 17, "Having difficulty concentrating?" },
                    { 60, 17, "Trouble falling or staying asleep?" },
                    { 61, 18, "Hallucinations" },
                    { 62, 18, "Delusions" },
                    { 63, 18, "Disorganized speech" },
                    { 64, 18, "Abnormal psychomotor behavior" },
                    { 65, 18, "Negative symptoms (restricted emotional expression or avolition)" },
                    { 66, 18, "Impaired cognition" },
                    { 67, 18, "Depression" },
                    { 68, 18, "Mania" },
                    { 69, 19, "How often do you have trouble wrapping up the final details of a project, once the challenging parts have been done?" },
                    { 70, 19, "How often do you have difficulty getting things in order when you have to do a task that requires organization?" },
                    { 71, 19, "How often do you have problems remembering appointments or obligations?" },
                    { 72, 19, "When you have a task that requires a lot of thought, how often do you avoid or delay getting started?" },
                    { 73, 19, "How often do you fidget or squirm with your hands or feet when you have to sit down for a long time?" },
                    { 74, 19, "How often do you feel overly active and compelled to do things, like you were driven by a motor?" },
                    { 75, 19, "How often do you make careless mistakes when you have to work on a boring or difficult project?" },
                    { 76, 19, "How often do you have difficulty keeping your attention when you are doing boring or repetitive work?" },
                    { 77, 19, "How often do you have difficulty concentrating on what people say to you, even when they are speaking to you directly?" },
                    { 78, 19, "How often do you misplace or have difficulty finding things at home or at work?" },
                    { 79, 19, "How often are you distracted by activity or noise around you?" },
                    { 80, 19, "How often do you leave your seat in meetings or other situations in which you are expected to remain seated?" },
                    { 81, 19, "How often do you feel restless or fidgety?" },
                    { 82, 19, "How often do you have difficulty unwinding and relaxing when you have time to yourself?" },
                    { 83, 19, "How often do you find yourself talking too much when you are in social situations?" },
                    { 84, 19, "When you're in a conversation, how often do you find yourself finishing the sentences of the people you are talking to, before they can finish them themselves?" },
                    { 85, 19, "How often do you have difficulty waiting your turn in situations when turn taking is required?" },
                    { 86, 19, "How often do you interrupt others when they are busy?" }
                });

            migrationBuilder.InsertData(
                table: "ScoringOptions",
                columns: new[] { "Id", "Description", "QuestionId", "Score" },
                values: new object[,]
                {
                    { 1, "None/Not at all", 1, 0 },
                    { 2, "Slight/Rare, less than a day or two", 1, 1 },
                    { 3, "Mild/Several days", 1, 2 },
                    { 4, "Moderate/More than half the days", 1, 3 },
                    { 5, "Severe/Nearly every day", 1, 4 },
                    { 6, "None/Not at all", 2, 0 },
                    { 7, "Slight/Rare, less than a day or two", 2, 1 },
                    { 8, "Mild/Several days", 2, 2 },
                    { 9, "Moderate/More than half the days", 2, 3 },
                    { 10, "Severe/Nearly every day", 2, 4 },
                    { 11, "None/Not at all", 3, 0 },
                    { 12, "Slight/Rare, less than a day or two", 3, 1 },
                    { 13, "Mild/Several days", 3, 2 },
                    { 14, "Moderate/More than half the days", 3, 3 },
                    { 15, "Severe/Nearly every day", 3, 4 },
                    { 16, "None/Not at all", 4, 0 },
                    { 17, "Slight/Rare, less than a day or two", 4, 1 },
                    { 18, "Mild/Several days", 4, 2 },
                    { 19, "Moderate/More than half the days", 4, 3 },
                    { 20, "Severe/Nearly every day", 4, 4 },
                    { 21, "None/Not at all", 5, 0 },
                    { 22, "Slight/Rare, less than a day or two", 5, 1 },
                    { 23, "Mild/Several days", 5, 2 },
                    { 24, "Moderate/More than half the days", 5, 3 },
                    { 25, "Severe/Nearly every day", 5, 4 },
                    { 26, "None/Not at all", 6, 0 },
                    { 27, "Slight/Rare, less than a day or two", 6, 1 },
                    { 28, "Mild/Several days", 6, 2 },
                    { 29, "Moderate/More than half the days", 6, 3 },
                    { 30, "Severe/Nearly every day", 6, 4 },
                    { 31, "None/Not at all", 7, 0 },
                    { 32, "Slight/Rare, less than a day or two", 7, 1 },
                    { 33, "Mild/Several days", 7, 2 },
                    { 34, "Moderate/More than half the days", 7, 3 },
                    { 35, "Severe/Nearly every day", 7, 4 },
                    { 36, "None/Not at all", 8, 0 },
                    { 37, "Slight/Rare, less than a day or two", 8, 1 },
                    { 38, "Mild/Several days", 8, 2 },
                    { 39, "Moderate/More than half the days", 8, 3 },
                    { 40, "Severe/Nearly every day", 8, 4 },
                    { 41, "None/Not at all", 9, 0 },
                    { 42, "Slight/Rare, less than a day or two", 9, 1 },
                    { 43, "Mild/Several days", 9, 2 },
                    { 44, "Moderate/More than half the days", 9, 3 },
                    { 45, "Severe/Nearly every day", 9, 4 },
                    { 46, "None/Not at all", 10, 0 },
                    { 47, "Slight/Rare, less than a day or two", 10, 1 },
                    { 48, "Mild/Several days", 10, 2 },
                    { 49, "Moderate/More than half the days", 10, 3 },
                    { 50, "Severe/Nearly every day", 10, 4 },
                    { 51, "None/Not at all", 11, 0 },
                    { 52, "Slight/Rare, less than a day or two", 11, 1 },
                    { 53, "Mild/Several days", 11, 2 },
                    { 54, "Moderate/More than half the days", 11, 3 },
                    { 55, "Severe/Nearly every day", 11, 4 },
                    { 56, "None/Not at all", 12, 0 },
                    { 57, "Slight/Rare, less than a day or two", 12, 1 },
                    { 58, "Mild/Several days", 12, 2 },
                    { 59, "Moderate/More than half the days", 12, 3 },
                    { 60, "Severe/Nearly every day", 12, 4 },
                    { 61, "None/Not at all", 13, 0 },
                    { 62, "Slight/Rare, less than a day or two", 13, 1 },
                    { 63, "Mild/Several days", 13, 2 },
                    { 64, "Moderate/More than half the days", 13, 3 },
                    { 65, "Severe/Nearly every day", 13, 4 },
                    { 66, "None/Not at all", 14, 0 },
                    { 67, "Slight/Rare, less than a day or two", 14, 1 },
                    { 68, "Mild/Several days", 14, 2 },
                    { 69, "Moderate/More than half the days", 14, 3 },
                    { 70, "Severe/Nearly every day", 14, 4 },
                    { 71, "None/Not at all", 15, 0 },
                    { 72, "Slight/Rare, less than a day or two", 15, 1 },
                    { 73, "Mild/Several days", 15, 2 },
                    { 74, "Moderate/More than half the days", 15, 3 },
                    { 75, "Severe/Nearly every day", 15, 4 },
                    { 76, "None/Not at all", 16, 0 },
                    { 77, "Slight/Rare, less than a day or two", 16, 1 },
                    { 78, "Mild/Several days", 16, 2 },
                    { 79, "Moderate/More than half the days", 16, 3 },
                    { 80, "Severe/Nearly every day", 16, 4 },
                    { 81, "None/Not at all", 17, 0 },
                    { 82, "Slight/Rare, less than a day or two", 17, 1 },
                    { 83, "Mild/Several days", 17, 2 },
                    { 84, "Moderate/More than half the days", 17, 3 },
                    { 85, "Severe/Nearly every day", 17, 4 },
                    { 86, "None/Not at all", 18, 0 },
                    { 87, "Slight/Rare, less than a day or two", 18, 1 },
                    { 88, "Mild/Several days", 18, 2 },
                    { 89, "Moderate/More than half the days", 18, 3 },
                    { 90, "Severe/Nearly every day", 18, 4 },
                    { 91, "None/Not at all", 19, 0 },
                    { 92, "Slight/Rare, less than a day or two", 19, 1 },
                    { 93, "Mild/Several days", 19, 2 },
                    { 94, "Moderate/More than half the days", 19, 3 },
                    { 95, "Severe/Nearly every day", 19, 4 },
                    { 96, "None/Not at all", 20, 0 },
                    { 97, "Slight/Rare, less than a day or two", 20, 1 },
                    { 98, "Mild/Several days", 20, 2 },
                    { 99, "Moderate/More than half the days", 20, 3 },
                    { 100, "Severe/Nearly every day", 20, 4 },
                    { 101, "None/Not at all", 21, 0 },
                    { 102, "Slight/Rare, less than a day or two", 21, 1 },
                    { 103, "Mild/Several days", 21, 2 },
                    { 104, "Moderate/More than half the days", 21, 3 },
                    { 105, "Severe/Nearly every day", 21, 4 },
                    { 106, "None/Not at all", 22, 0 },
                    { 107, "Slight/Rare, less than a day or two", 22, 1 },
                    { 108, "Mild/Several days", 22, 2 },
                    { 109, "Moderate/More than half the days", 22, 3 },
                    { 110, "Severe/Nearly every day", 22, 4 },
                    { 111, "None/Not at all", 23, 0 },
                    { 112, "Slight/Rare, less than a day or two", 23, 1 },
                    { 113, "Mild/Several days", 23, 2 },
                    { 114, "Moderate/More than half the days", 23, 3 },
                    { 115, "Severe/Nearly every day", 23, 4 },
                    { 116, "Never", 24, 1 },
                    { 117, "Rarely", 24, 2 },
                    { 118, "Sometimes", 24, 3 },
                    { 119, "Often", 24, 4 },
                    { 120, "Always", 24, 5 },
                    { 121, "Never", 25, 1 },
                    { 122, "Rarely", 25, 2 },
                    { 123, "Sometimes", 25, 3 },
                    { 124, "Often", 25, 4 },
                    { 125, "Always", 25, 5 },
                    { 126, "Never", 26, 1 },
                    { 127, "Rarely", 26, 2 },
                    { 128, "Sometimes", 26, 3 },
                    { 129, "Often", 26, 4 },
                    { 130, "Always", 26, 5 },
                    { 131, "Never", 27, 1 },
                    { 132, "Rarely", 27, 2 },
                    { 133, "Sometimes", 27, 3 },
                    { 134, "Often", 27, 4 },
                    { 135, "Always", 27, 5 },
                    { 136, "Never", 28, 1 },
                    { 137, "Rarely", 28, 2 },
                    { 138, "Sometimes", 28, 3 },
                    { 139, "Often", 28, 4 },
                    { 140, "Always", 28, 5 },
                    { 141, "Never", 29, 1 },
                    { 142, "Rarely", 29, 2 },
                    { 143, "Sometimes", 29, 3 },
                    { 144, "Often", 29, 4 },
                    { 145, "Always", 29, 5 },
                    { 146, "Never", 30, 1 },
                    { 147, "Rarely", 30, 2 },
                    { 148, "Sometimes", 30, 3 },
                    { 149, "Often", 30, 4 },
                    { 150, "Always", 30, 5 },
                    { 151, "Not at all", 31, 0 },
                    { 152, "Occasionally", 31, 1 },
                    { 153, "Often", 31, 2 },
                    { 154, "Frequently", 31, 3 },
                    { 155, "All the time", 31, 4 },
                    { 156, "Not at all", 32, 0 },
                    { 157, "Occasionally", 32, 1 },
                    { 158, "Often", 32, 2 },
                    { 159, "Frequently", 32, 3 },
                    { 160, "All the time", 32, 4 },
                    { 161, "Not at all", 33, 0 },
                    { 162, "Occasionally", 33, 1 },
                    { 163, "Often", 33, 2 },
                    { 164, "Frequently", 33, 3 },
                    { 165, "All the time", 33, 4 },
                    { 166, "Not at all", 34, 0 },
                    { 167, "Occasionally", 34, 1 },
                    { 168, "Often", 34, 2 },
                    { 169, "Frequently", 34, 3 },
                    { 170, "All the time", 34, 4 },
                    { 171, "Not at all", 35, 0 },
                    { 172, "Occasionally", 35, 1 },
                    { 173, "Often", 35, 2 },
                    { 174, "Frequently", 35, 3 },
                    { 175, "All the time", 35, 4 },
                    { 176, "None", 36, 0 },
                    { 177, "Mild", 36, 1 },
                    { 178, "Moderate", 36, 2 },
                    { 179, "Severe", 36, 3 },
                    { 180, "Extreme", 36, 4 },
                    { 181, "None", 37, 0 },
                    { 182, "Mild", 37, 1 },
                    { 183, "Moderate", 37, 2 },
                    { 184, "Severe", 37, 3 },
                    { 185, "Extreme", 37, 4 },
                    { 186, "None", 38, 0 },
                    { 187, "Mild", 38, 1 },
                    { 188, "Moderate", 38, 2 },
                    { 189, "Severe", 38, 3 },
                    { 190, "Extreme", 38, 4 },
                    { 191, "None", 39, 0 },
                    { 192, "Mild", 39, 1 },
                    { 193, "Moderate", 39, 2 },
                    { 194, "Severe", 39, 3 },
                    { 195, "Extreme", 39, 4 },
                    { 196, "None", 40, 0 },
                    { 197, "Mild", 40, 1 },
                    { 198, "Moderate", 40, 2 },
                    { 199, "Severe", 40, 3 },
                    { 200, "Extreme", 40, 4 },
                    { 201, "Not at all", 41, 0 },
                    { 202, "A little bit", 41, 1 },
                    { 203, "Moderately", 41, 2 },
                    { 204, "Quite a bit", 41, 3 },
                    { 205, "Extremely", 41, 4 },
                    { 206, "Not at all", 42, 0 },
                    { 207, "A little bit", 42, 1 },
                    { 208, "Moderately", 42, 2 },
                    { 209, "Quite a bit", 42, 3 },
                    { 210, "Extremely", 42, 4 },
                    { 211, "Not at all", 43, 0 },
                    { 212, "A little bit", 43, 1 },
                    { 213, "Moderately", 43, 2 },
                    { 214, "Quite a bit", 43, 3 },
                    { 215, "Extremely", 43, 4 },
                    { 216, "Not at all", 44, 0 },
                    { 217, "A little bit", 44, 1 },
                    { 218, "Moderately", 44, 2 },
                    { 219, "Quite a bit", 44, 3 },
                    { 220, "Extremely", 44, 4 },
                    { 221, "Not at all", 45, 0 },
                    { 222, "A little bit", 45, 1 },
                    { 223, "Moderately", 45, 2 },
                    { 224, "Quite a bit", 45, 3 },
                    { 225, "Extremely", 45, 4 },
                    { 226, "Not at all", 46, 0 },
                    { 227, "A little bit", 46, 1 },
                    { 228, "Moderately", 46, 2 },
                    { 229, "Quite a bit", 46, 3 },
                    { 230, "Extremely", 46, 4 },
                    { 231, "Not at all", 47, 0 },
                    { 232, "A little bit", 47, 1 },
                    { 233, "Moderately", 47, 2 },
                    { 234, "Quite a bit", 47, 3 },
                    { 235, "Extremely", 47, 4 },
                    { 236, "Not at all", 48, 0 },
                    { 237, "A little bit", 48, 1 },
                    { 238, "Moderately", 48, 2 },
                    { 239, "Quite a bit", 48, 3 },
                    { 240, "Extremely", 48, 4 },
                    { 241, "Not at all", 49, 0 },
                    { 242, "A little bit", 49, 1 },
                    { 243, "Moderately", 49, 2 },
                    { 244, "Quite a bit", 49, 3 },
                    { 245, "Extremely", 49, 4 },
                    { 246, "Not at all", 50, 0 },
                    { 247, "A little bit", 50, 1 },
                    { 248, "Moderately", 50, 2 },
                    { 249, "Quite a bit", 50, 3 },
                    { 250, "Extremely", 50, 4 },
                    { 251, "Not at all", 51, 0 },
                    { 252, "A little bit", 51, 1 },
                    { 253, "Moderately", 51, 2 },
                    { 254, "Quite a bit", 51, 3 },
                    { 255, "Extremely", 51, 4 },
                    { 256, "Not at all", 52, 0 },
                    { 257, "A little bit", 52, 1 },
                    { 258, "Moderately", 52, 2 },
                    { 259, "Quite a bit", 52, 3 },
                    { 260, "Extremely", 52, 4 },
                    { 261, "Not at all", 53, 0 },
                    { 262, "A little bit", 53, 1 },
                    { 263, "Moderately", 53, 2 },
                    { 264, "Quite a bit", 53, 3 },
                    { 265, "Extremely", 53, 4 },
                    { 266, "Not at all", 54, 0 },
                    { 267, "A little bit", 54, 1 },
                    { 268, "Moderately", 54, 2 },
                    { 269, "Quite a bit", 54, 3 },
                    { 270, "Extremely", 54, 4 },
                    { 271, "Not at all", 55, 0 },
                    { 272, "A little bit", 55, 1 },
                    { 273, "Moderately", 55, 2 },
                    { 274, "Quite a bit", 55, 3 },
                    { 275, "Extremely", 55, 4 },
                    { 276, "Not at all", 56, 0 },
                    { 277, "A little bit", 56, 1 },
                    { 278, "Moderately", 56, 2 },
                    { 279, "Quite a bit", 56, 3 },
                    { 280, "Extremely", 56, 4 },
                    { 281, "Not at all", 57, 0 },
                    { 282, "A little bit", 57, 1 },
                    { 283, "Moderately", 57, 2 },
                    { 284, "Quite a bit", 57, 3 },
                    { 285, "Extremely", 57, 4 },
                    { 286, "Not at all", 58, 0 },
                    { 287, "A little bit", 58, 1 },
                    { 288, "Moderately", 58, 2 },
                    { 289, "Quite a bit", 58, 3 },
                    { 290, "Extremely", 58, 4 },
                    { 291, "Not at all", 59, 0 },
                    { 292, "A little bit", 59, 1 },
                    { 293, "Moderately", 59, 2 },
                    { 294, "Quite a bit", 59, 3 },
                    { 295, "Extremely", 59, 4 },
                    { 296, "Not at all", 60, 0 },
                    { 297, "A little bit", 60, 1 },
                    { 298, "Moderately", 60, 2 },
                    { 299, "Quite a bit", 60, 3 },
                    { 300, "Extremely", 60, 4 },
                    { 301, "Not present", 61, 0 },
                    { 302, "Equivocal", 61, 1 },
                    { 303, "Present, but mild", 61, 2 },
                    { 304, "Present and moderate", 61, 3 },
                    { 305, "Present and severe", 61, 4 },
                    { 306, "Not present", 62, 0 },
                    { 307, "Equivocal", 62, 1 },
                    { 308, "Present, but mild", 62, 2 },
                    { 309, "Present and moderate", 62, 3 },
                    { 310, "Present and severe", 62, 4 },
                    { 311, "Not present", 63, 0 },
                    { 312, "Equivocal", 63, 1 },
                    { 313, "Present, but mild", 63, 2 },
                    { 314, "Present and moderate", 63, 3 },
                    { 315, "Present and severe", 63, 4 },
                    { 316, "Not present", 64, 0 },
                    { 317, "Equivocal", 64, 1 },
                    { 318, "Present, but mild", 64, 2 },
                    { 319, "Present and moderate", 64, 3 },
                    { 320, "Present and severe", 64, 4 },
                    { 321, "Not present", 65, 0 },
                    { 322, "Equivocal", 65, 1 },
                    { 323, "Present, but mild", 65, 2 },
                    { 324, "Present and moderate", 65, 3 },
                    { 325, "Present and severe", 65, 4 },
                    { 326, "Not present", 66, 0 },
                    { 327, "Equivocal", 66, 1 },
                    { 328, "Present, but mild", 66, 2 },
                    { 329, "Present and moderate", 66, 3 },
                    { 330, "Present and severe", 66, 4 },
                    { 331, "Not present", 67, 0 },
                    { 332, "Equivocal", 67, 1 },
                    { 333, "Present, but mild", 67, 2 },
                    { 334, "Present and moderate", 67, 3 },
                    { 335, "Present and severe", 67, 4 },
                    { 336, "Not present", 68, 0 },
                    { 337, "Equivocal", 68, 1 },
                    { 338, "Present, but mild", 68, 2 },
                    { 339, "Present and moderate", 68, 3 },
                    { 340, "Present and severe", 68, 4 },
                    { 341, "Never", 69, 0 },
                    { 342, "Rarely", 69, 1 },
                    { 343, "Sometimes", 69, 2 },
                    { 344, "Often", 69, 3 },
                    { 345, "Very Often", 69, 4 },
                    { 346, "Never", 70, 0 },
                    { 347, "Rarely", 70, 1 },
                    { 348, "Sometimes", 70, 2 },
                    { 349, "Often", 70, 3 },
                    { 350, "Very Often", 70, 4 },
                    { 351, "Never", 71, 0 },
                    { 352, "Rarely", 71, 1 },
                    { 353, "Sometimes", 71, 2 },
                    { 354, "Often", 71, 3 },
                    { 355, "Very Often", 71, 4 },
                    { 356, "Never", 72, 0 },
                    { 357, "Rarely", 72, 1 },
                    { 358, "Sometimes", 72, 2 },
                    { 359, "Often", 72, 3 },
                    { 360, "Very Often", 72, 4 },
                    { 361, "Never", 73, 0 },
                    { 362, "Rarely", 73, 1 },
                    { 363, "Sometimes", 73, 2 },
                    { 364, "Often", 73, 3 },
                    { 365, "Very Often", 73, 4 },
                    { 366, "Never", 74, 0 },
                    { 367, "Rarely", 74, 1 },
                    { 368, "Sometimes", 74, 2 },
                    { 369, "Often", 74, 3 },
                    { 370, "Very Often", 74, 4 },
                    { 371, "Never", 75, 0 },
                    { 372, "Rarely", 75, 1 },
                    { 373, "Sometimes", 75, 2 },
                    { 374, "Often", 75, 3 },
                    { 375, "Very Often", 75, 4 },
                    { 376, "Never", 76, 0 },
                    { 377, "Rarely", 76, 1 },
                    { 378, "Sometimes", 76, 2 },
                    { 379, "Often", 76, 3 },
                    { 380, "Very Often", 76, 4 },
                    { 381, "Never", 77, 0 },
                    { 382, "Rarely", 77, 1 },
                    { 383, "Sometimes", 77, 2 },
                    { 384, "Often", 77, 3 },
                    { 385, "Very Often", 77, 4 },
                    { 386, "Never", 78, 0 },
                    { 387, "Rarely", 78, 1 },
                    { 388, "Sometimes", 78, 2 },
                    { 389, "Often", 78, 3 },
                    { 390, "Very Often", 78, 4 },
                    { 391, "Never", 79, 0 },
                    { 392, "Rarely", 79, 1 },
                    { 393, "Sometimes", 79, 2 },
                    { 394, "Often", 79, 3 },
                    { 395, "Very Often", 79, 4 },
                    { 396, "Never", 80, 0 },
                    { 397, "Rarely", 80, 1 },
                    { 398, "Sometimes", 80, 2 },
                    { 399, "Often", 80, 3 },
                    { 400, "Very Often", 80, 4 },
                    { 401, "Never", 81, 0 },
                    { 402, "Rarely", 81, 1 },
                    { 403, "Sometimes", 81, 2 },
                    { 404, "Often", 81, 3 },
                    { 405, "Very Often", 81, 4 },
                    { 406, "Never", 82, 0 },
                    { 407, "Rarely", 82, 1 },
                    { 408, "Sometimes", 82, 2 },
                    { 409, "Often", 82, 3 },
                    { 410, "Very Often", 82, 4 },
                    { 411, "Never", 83, 0 },
                    { 412, "Rarely", 83, 1 },
                    { 413, "Sometimes", 83, 2 },
                    { 414, "Often", 83, 3 },
                    { 415, "Very Often", 83, 4 },
                    { 416, "Never", 84, 0 },
                    { 417, "Rarely", 84, 1 },
                    { 418, "Sometimes", 84, 2 },
                    { 419, "Often", 84, 3 },
                    { 420, "Very Often", 84, 4 },
                    { 421, "Never", 85, 0 },
                    { 422, "Rarely", 85, 1 },
                    { 423, "Sometimes", 85, 2 },
                    { 424, "Often", 85, 3 },
                    { 425, "Very Often", 85, 4 },
                    { 426, "Never", 86, 0 },
                    { 427, "Rarely", 86, 1 },
                    { 428, "Sometimes", 86, 2 },
                    { 429, "Often", 86, 3 },
                    { 430, "Very Often", 86, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_AssessmentId",
                table: "Answers",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_DomainId",
                table: "Assessments",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_UserId",
                table: "Assessments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalityTestAnswers_TraitId",
                table: "PersonalityTestAnswers",
                column: "TraitId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_DomainId",
                table: "Questions",
                column: "DomainId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringOptions_QuestionId",
                table: "ScoringOptions",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "PersonalityTestAnswers");

            migrationBuilder.DropTable(
                name: "ScoringOptions");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "PersonalityTraits");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Domains");
        }
    }
}
