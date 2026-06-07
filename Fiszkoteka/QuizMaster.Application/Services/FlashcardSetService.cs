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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
