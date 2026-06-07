using Microsoft.EntityFrameworkCore;
using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Commands.Flashcards;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Application.Services
{
    public sealed class FlashcardService : IFlashcardService
    {
        private readonly IQuizMasterDbContext _context;

        public FlashcardService(IQuizMasterDbContext context)
        {
            _context = context;
        }

        public async Task<Flashcard> CreateFlashcard(
            CreateFlashcardCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.Question))
                throw new EmptyFieldException("Pytanie");

            if (string.IsNullOrWhiteSpace(command.Answer))
                throw new EmptyFieldException("Odpowiedź");

            var flashcardSet = await _context.FlashcardSets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == command.FlashcardSetId, cancellationToken);

            if (flashcardSet == null)
                throw new FlashcardSetNotFoundException(command.FlashcardSetId);

            if (flashcardSet.UserId != command.UserId)
                throw new FlashcardSetAccessDeniedException();

            var flashcard = new Flashcard
            {
                FlashcardSetId = command.FlashcardSetId,
                Question = command.Question.Trim(),
                Answer = command.Answer.Trim(),
                Hint = string.IsNullOrWhiteSpace(command.Hint) ? null : command.Hint.Trim(),
                Difficulty = command.Difficulty
            };

            var result = await _context.Flashcards.AddAsync(
                flashcard,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }

        public async Task DeleteFlashcard(
            int id,
            int userId,
            CancellationToken cancellationToken = default)
        {
            var flashcard = await _context.Flashcards
                .Include(x => x.FlashcardSet)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (flashcard == null)
                throw new FlashcardNotFoundException(id);

            if (flashcard.FlashcardSet.UserId != userId)
                throw new FlashcardAccessDeniedException();

            _context.Flashcards.Remove(flashcard);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Flashcard>> GetFlashcards(
            int userId,
            CancellationToken cancellationToken = default)
        {
            var flashcards = await _context.Flashcards
                .AsNoTracking()
                .Include(x => x.FlashcardSet)
                .Where(x => x.FlashcardSet.UserId == userId)
                .OrderBy(x => x.FlashcardSet.Name)
                .ThenBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return flashcards;
        }

        public async Task UpdateFlashcard(
            int id,
            UpdateFlashcardCommand command,
            CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var flashcard = await _context.Flashcards
                .Include(x => x.FlashcardSet)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (flashcard == null)
                throw new FlashcardNotFoundException(id);

            if (flashcard.FlashcardSet.UserId != command.UserId)
                throw new FlashcardAccessDeniedException();

            if (command.Question != null)
            {
                if (string.IsNullOrWhiteSpace(command.Question))
                    throw new EmptyFieldException("Pytanie");

                flashcard.Question = command.Question.Trim();
            }

            if (command.Answer != null)
            {
                if (string.IsNullOrWhiteSpace(command.Answer))
                    throw new EmptyFieldException("Odpowiedź");

                flashcard.Answer = command.Answer.Trim();
            }

            if (command.Hint != null)
            {
                flashcard.Hint = string.IsNullOrWhiteSpace(command.Hint) ? null : command.Hint.Trim();
            }

            if (command.Difficulty.HasValue)
            {
                flashcard.Difficulty = command.Difficulty.Value;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
