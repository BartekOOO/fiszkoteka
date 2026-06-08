using Microsoft.EntityFrameworkCore;
using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Commands.Learning;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Dto;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

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

            if (!flashcardSet.IsPublic && flashcardSet.UserId != command.UserId)
                throw new FlashcardSetAccessDeniedException();

            var totalFlashcardsCount = flashcardSet.Flashcards.Count;

            if (totalFlashcardsCount == 0)
                throw new EmptyFlashcardSetException(command.FlashcardSetId);

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

            var result = await _context.LearningSessions.AddAsync(
                session,
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

            var alreadyReviewedFlashcardIds = await _context.UserFlashcardProgresses
                .AsNoTracking()
                .Where(x =>
                    x.UserId == userId &&
                    x.Flashcard.FlashcardSetId == session.FlashcardSetId)
                .Select(x => x.FlashcardId)
                .ToListAsync(cancellationToken);

            var nextFlashcard = await _context.Flashcards
                .AsNoTracking()
                .Where(x =>
                    x.FlashcardSetId == session.FlashcardSetId &&
                    !alreadyReviewedFlashcardIds.Contains(x.Id))
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (nextFlashcard == null)
                return null;

            return new LearningFlashcardDto
            {
                Id = nextFlashcard.Id,
                FlashcardSetId = nextFlashcard.FlashcardSetId,
                Question = nextFlashcard.Question,
                Hint = nextFlashcard.Hint,
                Difficulty = nextFlashcard.Difficulty
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

            var flashcard = await _context.Flashcards
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == command.FlashcardId &&
                    x.FlashcardSetId == session.FlashcardSetId,
                    cancellationToken);

            if (flashcard == null)
                throw new FlashcardNotFoundException(command.FlashcardId);

            var progress = await _context.UserFlashcardProgresses
                .FirstOrDefaultAsync(x =>
                    x.UserId == command.UserId &&
                    x.FlashcardId == command.FlashcardId,
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

                await _context.UserFlashcardProgresses.AddAsync(progress, cancellationToken);
            }

            if (command.IsCorrect)
            {
                progress.CorrectAnswersCount++;
                progress.MasteryLevel++;
                session.CorrectAnswersCount++;
            }
            else
            {
                progress.WrongAnswersCount++;
                progress.MasteryLevel--;

                if (progress.MasteryLevel < 0)
                    progress.MasteryLevel = 0;

                session.WrongAnswersCount++;
            }

            progress.LastReviewedAt = DateTime.UtcNow;
            progress.NextReviewAt = CalculateNextReviewAt(progress.MasteryLevel);

            session.ReviewedFlashcardsCount++;

            if (session.ReviewedFlashcardsCount >= session.TotalFlashcardsCount)
            {
                session.FinishedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new AnswerFlashcardResultDto
            {
                FlashcardId = flashcard.Id,
                IsCorrect = command.IsCorrect,
                CorrectAnswer = flashcard.Answer,
                ReviewedFlashcardsCount = session.ReviewedFlashcardsCount,
                CorrectAnswersCount = session.CorrectAnswersCount,
                WrongAnswersCount = session.WrongAnswersCount,
                SessionFinished = session.IsFinished
            };
        }

        public async Task<LearningSessionDto> FinishLearningSession(
            int sessionId,
            int userId,
            CancellationToken cancellationToken = default)
        {
            var session = await _context.LearningSessions
                .Include(x => x.FlashcardSet)
                .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);

            if (session == null)
                throw new LearningSessionNotFoundException(sessionId);

            if (session.UserId != userId)
                throw new FlashcardSetAccessDeniedException();

            if (!session.IsFinished)
            {
                session.FinishedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return ToDto(session, session.FlashcardSet.Name);
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
    }
}

