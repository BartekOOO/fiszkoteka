using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QuizMaster.Contracts.Dto
{
    [Description("Główny dashboard - Dzisiejszy cel")]
    public sealed class DailyGoalDto
    {
        public int TargetFlashcardsCount { get; set; }
        public int ReviewedFlashcardsCount { get; set; }

        public int ProgressPercent
        {
            get
            {
                if (TargetFlashcardsCount <= 0)
                    return 0;

                return Math.Min(100, ReviewedFlashcardsCount * 100 / TargetFlashcardsCount);
            }
        }
    }
}
