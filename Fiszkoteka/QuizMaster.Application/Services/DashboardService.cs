using Microsoft.EntityFrameworkCore;
using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Dto;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Application.Services
{
    public sealed class DashboardService : IDashboardService
    {
        private const int DailyGoalTarget = 30;

        private readonly IQuizMasterDbContext _context;

        public DashboardService(IQuizMasterDbContext context)
        {
            _context = context;
        }

        public async Task<MainDashboardDto> GetDashboard(
            int userId,
            CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
                throw new InvalidLoginException();

            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var flashcardSetsCount = await _context.FlashcardSets
                .AsNoTracking()
                .CountAsync(x => x.UserId == userId, cancellationToken);

            var flashcardsCount = await _context.Flashcards
                .AsNoTracking()
                .CountAsync(x => x.FlashcardSet.UserId == userId, cancellationToken);

            var todayReviewedFlashcardsCount = await _context.LearningSessionItems
                .AsNoTracking()
                .Include(x => x.LearningSession)
                .CountAsync(x =>
                    x.LearningSession.UserId == userId &&
                    x.AnsweredAt >= today &&
                    x.AnsweredAt < tomorrow,
                    cancellationToken);

            var totalCorrectAnswers = await _context.UserFlashcardProgresses
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .SumAsync(x => x.CorrectAnswersCount, cancellationToken);

            var totalWrongAnswers = await _context.UserFlashcardProgresses
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .SumAsync(x => x.WrongAnswersCount, cancellationToken);

            var allAnswers = totalCorrectAnswers + totalWrongAnswers;

            var effectivenessPercent = allAnswers == 0
                ? 0
                : totalCorrectAnswers * 100 / allAnswers;

            var recentSets = await _context.FlashcardSets
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new RecentFlashcardSetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    FlashcardsCount = x.Flashcards.Count,

                    LastLearningAt = _context.LearningSessions
                        .Where(s =>
                            s.UserId == userId &&
                            s.FlashcardSetId == x.Id)
                        .OrderByDescending(s => s.StartedAt)
                        .Select(s => (DateTime?)s.StartedAt)
                        .FirstOrDefault()
                })
                .OrderByDescending(x => x.LastLearningAt)
                .ThenBy(x => x.Name)
                .Take(3)
                .ToListAsync(cancellationToken);

            foreach (var set in recentSets)
            {
                set.LastLearningText = CreateLastLearningText(set.LastLearningAt);
            }

            var activeSessions = await _context.LearningSessions
                .AsNoTracking()
                .Where(x =>
                    x.UserId == userId &&
                    x.FinishedAt == null)
                .OrderByDescending(x => x.StartedAt)
                .Take(5)
                .Select(x => new ActiveLearningSessionDto
                {
                    Id = x.Id,
                    FlashcardSetId = x.FlashcardSetId,
                    FlashcardSetName = x.FlashcardSet.Name,
                    TotalFlashcardsCount = x.TotalFlashcardsCount,
                    ReviewedFlashcardsCount = x.ReviewedFlashcardsCount,
                    StartedAt = x.StartedAt
                })
                .ToListAsync(cancellationToken);

            var learningStreakDays = await CalculateLearningStreakDays(
                userId,
                cancellationToken);

            return new MainDashboardDto
            {
                UserName = user.UserName,
                Email = user.Email,

                FlashcardSetsCount = flashcardSetsCount,
                FlashcardsCount = flashcardsCount,

                LearningStreakDays = learningStreakDays,
                EffectivenessPercent = effectivenessPercent,

                DailyGoal = new DailyGoalDto
                {
                    TargetFlashcardsCount = DailyGoalTarget,
                    ReviewedFlashcardsCount = todayReviewedFlashcardsCount
                },

                RecentSets = recentSets,
                ActiveSessions = activeSessions
            };
        }

        private static string CreateLastLearningText(DateTime? lastLearningAt)
        {
            if (lastLearningAt == null)
                return "brak nauki";

            var date = lastLearningAt.Value.Date;
            var today = DateTime.UtcNow.Date;

            if (date == today)
                return "ostatnia nauka dzisiaj";

            if (date == today.AddDays(-1))
                return "ostatnia nauka wczoraj";

            return $"ostatnia nauka {date:dd.MM.yyyy}";
        }

        private async Task<int> CalculateLearningStreakDays(
            int userId,
            CancellationToken cancellationToken)
        {
            var dates = await _context.UserFlashcardProgresses
                .AsNoTracking()
                .Where(x =>
                    x.UserId == userId &&
                    x.LastReviewedAt != null)
                .Select(x => x.LastReviewedAt.Value.Date)
                .Distinct()
                .OrderByDescending(x => x)
                .ToListAsync(cancellationToken);

            if (dates.Count == 0)
                return 0;

            var streak = 0;
            var currentDate = DateTime.UtcNow.Date;

            foreach (var date in dates)
            {
                if (date == currentDate)
                {
                    streak++;
                    currentDate = currentDate.AddDays(-1);
                    continue;
                }

                if (date == currentDate.AddDays(-1) && streak == 0)
                {
                    streak++;
                    currentDate = date.AddDays(-1);
                    continue;
                }

                if (date == currentDate)
                {
                    streak++;
                    currentDate = currentDate.AddDays(-1);
                    continue;
                }

                break;
            }

            return streak;
        }
    }
}
