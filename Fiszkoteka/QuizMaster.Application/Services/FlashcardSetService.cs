using Microsoft.EntityFrameworkCore;
using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Contracts.Dto;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;
using System.Runtime.CompilerServices;

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

        public async Task<CopiedFlashcardSetDto> CopyFlashcardSet(
            int id,
            int userId,
            CancellationToken cancellationToken)
        {
            var sourceSet = await _context.FlashcardSets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (sourceSet is null)
                throw new FlashcardSetNotFoundException(id);

            if (sourceSet.UserId != userId && !sourceSet.IsPublic)
                throw new FlashcardSetAccessDeniedException();

            var sourceUserId = sourceSet.UserId;

            var copiedSet = new FlashcardSet
            {
                Name = sourceSet.Name,
                Description = sourceSet.Description,
                CategoryId = sourceSet.CategoryId,
                UserId = userId,
                IsPublic = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.FlashcardSets.Add(copiedSet);

            await _context.SaveChangesAsync(cancellationToken);

            var sourceFlashcards = await _context.Flashcards
                .AsNoTracking()
                .Where(x => x.FlashcardSetId == id)
                .ToListAsync(cancellationToken);

            var copiedFlashcards = sourceFlashcards
                .Select(x => new Flashcard
                {
                    Question = x.Question,
                    Answer = x.Answer,
                    FlashcardSetId = copiedSet.Id,
                    Hint = x.Hint ?? ""
                })
                .ToList();

            _context.Flashcards.AddRange(copiedFlashcards);

            var baseUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == sourceUserId, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new CopiedFlashcardSetDto
            {
                FromUser = baseUser.UserName,
                Id = copiedSet.Id,
                Response = "Pomyślnie udało się skopiować zestaw"
            };
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
            flashCardSet.CreatedAt = DateTime.Now;

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

            var hasActiveSession = await _context.LearningSessions
                .AsNoTracking()
                .AnyAsync(x =>
                    x.FlashcardSetId == id &&
                    x.FinishedAt == null,
                    cancellationToken);

            if (hasActiveSession)
                throw new ActiveLearningSessionExistsException();

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

        public async Task<List<FlashcardSetListItemDto>> GetFlashcardSets(
            int userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.FlashcardSets
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new FlashcardSetListItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,

                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,

                    FlashcardsCount = x.Flashcards.Count,

                    IsPublic = x.IsPublic,
                    CreatedAt = x.CreatedAt,
                    Author = x.User.UserName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<FlashcardSetListItemDto>> GetPublicFlashcardSets(
            string? userName,
            string? categoryName,
            int userId,
            CancellationToken cancellationToken)
        {
            var setsQuery = _context.FlashcardSets
                .AsNoTracking()
                .Where(x => x.IsPublic && x.UserId != userId);

            if (userName is not null)
            {
                var userIds = await _context.Users
                    .AsNoTracking()
                    .Where(x => EF.Functions.Like(x.UserName, $"%{userName}%"))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                if (userIds.Count == 0)
                    return new List<FlashcardSetListItemDto>();

                setsQuery = setsQuery.Where(x => userIds.Contains(x.UserId));
            }

            if (categoryName is not null)
            {
                var categoryIds = await _context.Categories
                    .AsNoTracking()
                    .Where(x => EF.Functions.Like(x.Name, $"%{categoryName}%"))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                if (categoryIds.Count == 0)
                    return new List<FlashcardSetListItemDto>();

                setsQuery = setsQuery.Where(x => categoryIds.Contains(x.CategoryId));
            }

            setsQuery = setsQuery
                .OrderByDescending(x => x.CreatedAt);

            var sets = await setsQuery.ToListAsync(cancellationToken);

            return await setsQuery
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new FlashcardSetListItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,

                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,

                    FlashcardsCount = x.Flashcards.Count,

                    IsPublic = x.IsPublic,
                    Author = x.User.UserName,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);
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
