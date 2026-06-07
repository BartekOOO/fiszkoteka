using Microsoft.EntityFrameworkCore;
using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;

namespace QuizMaster.Application.Services
{
    public sealed class FlashcardSetService : IFlashcardSetService
    {
        private readonly IQuizMasterDbContext _context;
        private readonly IAuthService _authService;

        public FlashcardSetService(IQuizMasterDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<FlashcardSet> CreateFlashcardSet(CreateFlashcardSetCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (String.IsNullOrWhiteSpace(command.Name))
                throw new EmptyFieldException("Nazwa");

            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == command.CategoryId);

            if (category == null)
                throw new CategoryNotFoundException(command.CategoryId);

            var flashCardSet = command.ToFlashcardSet();
            flashCardSet.UserId = command.UserId;

            var result = await _context.FlashcardSets.AddAsync(flashCardSet, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return result.Entity;
        }

        public async Task DeleteFlashcardSet(int id, int userId, CancellationToken cancellationToken = default)
        {
            var flashcardSet = await _context.FlashcardSets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (flashcardSet == null)
                throw new FlashcardSetNotFoundException(id);

            if (flashcardSet.UserId != userId)
                throw new FlashcardSetAccessDeniedException();

            _context.FlashcardSets.Remove(flashcardSet);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<FlashcardSet> GetFlashcardSetDetails(int id, int userId, CancellationToken cancellationToken = default)
        {
            var result = await _context.FlashcardSets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result == null)
                throw new FlashcardSetNotFoundException(id);

            if (result.UserId != userId)
                throw new FlashcardSetAccessDeniedException();

            result.Category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == result.CategoryId, cancellationToken)
                ?? throw new Exception("Nie udało się pobrać danych o kategorii");

            result.Flashcards = await _context.Flashcards
                .AsNoTracking()
                .Where(x => x.FlashcardSetId == id)
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<List<FlashcardSet>> GetFlashcardSets(int userId, CancellationToken cancellationToken = default)
        {
            var sets = await _context.FlashcardSets
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            return sets;
        }

        public async Task<List<FlashcardSet>> GetPublicFlashcardSets(CancellationToken cancellationToken)
        {
            var sets = await _context.FlashcardSets
                .AsNoTracking()
                .Where(x => x.IsPublic)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            return sets;
        }

        public async Task UpdateFlashcardSet(int id, UpdateFlashcardSetCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var flashcardSet = await _context.FlashcardSets
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (flashcardSet == null)
                throw new FlashcardSetNotFoundException(id);

            if (flashcardSet.UserId != command.UserId)
                throw new FlashcardSetAccessDeniedException();

            if (command.Name != null)
            {
                if (string.IsNullOrWhiteSpace(command.Name))
                    throw new EmptyFieldException("Nazwa");

                flashcardSet.Name = command.Name.Trim();
            }

            if (command.Description != null)
            {
                flashcardSet.Description = command.Description.Trim();
            }

            if (command.IsPublic.HasValue)
            {
                flashcardSet.IsPublic = command.IsPublic.Value;
            }

            if (command.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == command.CategoryId.Value, cancellationToken);

                if (!categoryExists)
                    throw new CategoryNotFoundException(command.CategoryId.Value);

                flashcardSet.CategoryId = command.CategoryId.Value;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
