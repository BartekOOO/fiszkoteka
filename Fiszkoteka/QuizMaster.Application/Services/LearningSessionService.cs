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

            foreach(var item in flashcardSet.Flashcards)
            {
                await _context.LearningSessionItems
                    .AddAsync(new LearningSessionItem()
                    {
                        LearningSessionId = result.Entity.Id,
                        FlashcardId = item.Id,
                        IsAnswered = false,
                        IsCorrect = null,
                        AnsweredAt = null
                    });
            }

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



            throw new NotImplementedException("W trakcie pracy");
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

        public async Task<List<LearningSessionDto>> GetLearningSessions(int userId, CancellationToken cancellationToken)
        {
            return await _context.LearningSessions
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => ToDto(x, x.FlashcardSet.Name))
                .ToListAsync(cancellationToken);
        }
    }
}

