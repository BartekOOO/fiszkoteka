using Microsoft.EntityFrameworkCore;
using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Commands.Learning;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Dto;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QuizMaster.Application.Services
{
    public sealed class LearningSessionService : ILearningSessionService
    {
        private readonly IQuizMasterDbContext _context;

        public LearningSessionService(IQuizMasterDbContext context)
        {
            _context = context;
        }

        public async Task<LearningSessionDto> StartLearningSession(
            StartLearningSessionCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var flashcardSet = await _context.FlashcardSets
                .AsNoTracking()
                .Include(x => x.Flashcards)
                .FirstOrDefaultAsync(x => x.Id == command.FlashcardSetId, cancellationToken);

            if (flashcardSet == null)
                throw new FlashcardSetNotFoundException(command.FlashcardSetId);

            if (flashcardSet.UserId != command.UserId)
                throw new FlashcardSetAccessDeniedException();

            var totalFlashcardsCount = flashcardSet.Flashcards.Count;

            if (totalFlashcardsCount == 0)
                throw new EmptyFlashcardSetException(command.FlashcardSetId);

            var sessionsCount = await _context.LearningSessions
                .Where(x => 
                    x.UserId == command.UserId
                    && x.FinishedAt == null)
                .CountAsync(cancellationToken);

            if (sessionsCount > 5)
                throw new TooManyActiveLearningSessionsException();

            var session = new LearningSession
            {
                UserId = command.UserId,
                FlashcardSetId = command.FlashcardSetId,
                StartedAt = DateTime.UtcNow,
                TotalFlashcardsCount = totalFlashcardsCount,
                ReviewedFlashcardsCount = 0,
                CorrectAnswersCount = 0,
                WrongAnswersCount = 0
            };

            await _context.LearningSessions.AddAsync(session, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var sessionItems = flashcardSet.Flashcards
                .Select(x => new LearningSessionItem
                {
                    LearningSessionId = session.Id,
                    FlashcardId = x.Id,
                    IsAnswered = false,
                    IsCorrect = null,
                    AnsweredAt = null
                })
                .ToList();

            await _context.LearningSessionItems.AddRangeAsync(
                sessionItems,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return ToDto(session, flashcardSet.Name);
        }

        public async Task<LearningSessionDto> GetLearningSession(
            int sessionId,
            int userId,
            CancellationToken cancellationToken = default)
        {
            var session = await _context.LearningSessions
                .AsNoTracking()
                .Include(x => x.FlashcardSet)
                .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);

            if (session == null)
                throw new LearningSessionNotFoundException(sessionId);

            if (session.UserId != userId)
                throw new FlashcardSetAccessDeniedException();

            if (session.IsFinished)
                throw new LearningSessionFinishedException();

            return ToDto(session, session.FlashcardSet.Name);
        }

        public async Task<LearningFlashcardDto> GetNextFlashcard(
            int sessionId,
            int userId,
            CancellationToken cancellationToken = default)
        {
            var session = await _context.LearningSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);

            if (session == null)
                throw new LearningSessionNotFoundException(sessionId);

            if (session.UserId != userId)
                throw new FlashcardSetAccessDeniedException();

            if (session.IsFinished)
                throw new LearningSessionFinishedException();

            var nextItem = await _context.LearningSessionItems
                .AsNoTracking()
                .Include(x => x.Flashcard)
                .Where(x =>
                    x.LearningSessionId == session.Id &&
                    !x.IsAnswered)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (nextItem == null)
                throw new LearningSessionExhaustedException();

            var flashcard = nextItem.Flashcard;

            if (flashcard == null)
                throw new FlashcardNotFoundException(nextItem.FlashcardId);

            return new LearningFlashcardDto
            {
                Id = flashcard.Id,
                FlashcardSetId = flashcard.FlashcardSetId,
                Question = flashcard.Question,
                Hint = flashcard.Hint,
                Difficulty = flashcard.Difficulty,
                Answer = flashcard.Answer,
            };
        }

        public async Task<AnswerFlashcardResultDto> AnswerFlashcard(
            int sessionId,
            AnswerFlashcardCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var session = await _context.LearningSessions
                .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);

            if (session == null)
                throw new LearningSessionNotFoundException(sessionId);

            if (session.UserId != command.UserId)
                throw new FlashcardSetAccessDeniedException();

            if (session.IsFinished)
                throw new LearningSessionFinishedException();

            var sessionItem = await _context.LearningSessionItems
                .Include(x => x.Flashcard)
                .FirstOrDefaultAsync(x =>
                    x.LearningSessionId == sessionId &&
                    x.FlashcardId == command.FlashcardId,
                    cancellationToken);

            if (sessionItem == null)
                throw new FlashcardNotFoundException(command.FlashcardId);

            if (sessionItem.IsAnswered)
                throw new Exception("Ta fiszka została już odpowiedziana w tej sesji.");

            var flashcard = sessionItem.Flashcard;

            if (flashcard == null)
                throw new FlashcardNotFoundException(command.FlashcardId);

            var progress = await _context.UserFlashcardProgresses
                .FirstOrDefaultAsync(x =>
                    x.FlashcardId == command.FlashcardId &&
                    x.UserId == command.UserId,
                    cancellationToken);

            if (progress == null)
            {
                progress = new UserFlashcardProgress
                {
                    UserId = command.UserId,
                    FlashcardId = command.FlashcardId,
                    CorrectAnswersCount = 0,
                    WrongAnswersCount = 0,
                    MasteryLevel = 0
                };

                await _context.UserFlashcardProgresses.AddAsync(
                    progress,
                    cancellationToken);
            }

            sessionItem.IsAnswered = true;
            sessionItem.IsCorrect = command.IsCorrect;
            sessionItem.AnsweredAt = DateTime.UtcNow;

            session.ReviewedFlashcardsCount++;

            if (command.IsCorrect)
            {
                progress.MasteryLevel++;
                progress.CorrectAnswersCount++;

                session.CorrectAnswersCount++;
            }
            else
            {
                progress.MasteryLevel = Math.Max(0, progress.MasteryLevel - 1);
                progress.WrongAnswersCount++;

                session.WrongAnswersCount++;
            }

            progress.LastReviewedAt = DateTime.UtcNow;
            progress.NextReviewAt = CalculateNextReviewAt(progress.MasteryLevel);

            var hasUnansweredItems = await _context.LearningSessionItems
                .AsNoTracking()
                .AnyAsync(x =>
                    x.LearningSessionId == sessionId &&
                    x.Id != sessionItem.Id &&
                    !x.IsAnswered,
                    cancellationToken);

            var isSessionFinished = !hasUnansweredItems;

            if (isSessionFinished)
                session.FinishedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new AnswerFlashcardResultDto
            {
                FlashcardId = command.FlashcardId,
                ReviewedFlashcardsCount = session.ReviewedFlashcardsCount,
                CorrectAnswersCount = session.CorrectAnswersCount,
                WrongAnswersCount = session.WrongAnswersCount,
                SessionFinished = isSessionFinished
            };
        }

        private static DateTime CalculateNextReviewAt(int masteryLevel)
        {
            var now = DateTime.UtcNow;

            if (masteryLevel <= 0)
                return now.AddMinutes(10);

            if (masteryLevel == 1)
                return now.AddHours(1);

            if (masteryLevel == 2)
                return now.AddDays(1);

            if (masteryLevel == 3)
                return now.AddDays(3);

            if (masteryLevel == 4)
                return now.AddDays(7);

            return now.AddDays(14);
        }

        private static LearningSessionDto ToDto(
            LearningSession session,
            string flashcardSetName)
        {
            return new LearningSessionDto
            {
                Id = session.Id,
                FlashcardSetId = session.FlashcardSetId,
                FlashcardSetName = flashcardSetName,
                StartedAt = session.StartedAt,
                FinishedAt = session.FinishedAt,
                TotalFlashcardsCount = session.TotalFlashcardsCount,
                ReviewedFlashcardsCount = session.ReviewedFlashcardsCount,
                CorrectAnswersCount = session.CorrectAnswersCount,
                WrongAnswersCount = session.WrongAnswersCount,
                IsFinished = session.IsFinished
            };
        }

        public async Task<List<LearningSessionDto>> GetLearningSessions(
            int userId,
            CancellationToken cancellationToken)
        {
            return await _context.LearningSessions
                .AsNoTracking()
                .Where(x => x.UserId == userId && !x.IsFinished)
                .OrderByDescending(x => x.StartedAt)
                .Select(x => new LearningSessionDto
                {
                    Id = x.Id,
                    FlashcardSetId = x.FlashcardSetId,
                    FlashcardSetName = x.FlashcardSet.Name,
                    StartedAt = x.StartedAt,
                    FinishedAt = x.FinishedAt,
                    TotalFlashcardsCount = x.TotalFlashcardsCount,
                    ReviewedFlashcardsCount = x.ReviewedFlashcardsCount,
                    CorrectAnswersCount = x.CorrectAnswersCount,
                    WrongAnswersCount = x.WrongAnswersCount,
                    IsFinished = x.FinishedAt.HasValue
                })
                .ToListAsync(cancellationToken);
        }

        public async Task FinishSession(int id, int userId, CancellationToken cancellationToken)
        {
            var session = await _context.LearningSessions
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (session == null)
                throw new LearningSessionNotFoundException(id);

            if (session.UserId != userId)
                throw new FlashcardSetAccessDeniedException();

            if (session.IsFinished)
                throw new LearningSessionFinishedException();

            session.FinishedAt = DateTime.Now;

            var sessionItemsToDelete = await _context.LearningSessionItems
                .Where(x => x.LearningSessionId == id 
                            && x.AnsweredAt == null)
                .ToListAsync(cancellationToken);

            _context.LearningSessionItems
                .RemoveRange(sessionItemsToDelete);

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}

