using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QuizMaster.Contracts.Dto
{
    [Description("Główny dashboard")]
    public sealed class MainDashboardDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public int FlashcardSetsCount { get; set; }
        public int FlashcardsCount { get; set; }

        public int LearningStreakDays { get; set; }
        public int EffectivenessPercent { get; set; }

        public DailyGoalDto DailyGoal { get; set; }

        public List<RecentFlashcardSetDto> RecentSets { get; set; } = new();
        public List<ActiveLearningSessionDto> ActiveSessions { get; set; } = new();
    }
}
