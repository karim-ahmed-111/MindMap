﻿using MindMap.Models;

namespace Mind_Map.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }

        public string? PersonalityType { get; set; }
        public List<Assessment>? Assessments { get; set; } = new List<Assessment>();

    }
}
