using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Commands.FlashcardSets;
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
            throw new NotImplementedException();
        }

        public async Task DeleteFlashcardSet(int id, int userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<FlashcardSet> GetFlashcardSetDetails(int id, int userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FlashcardSet>> GetFlashcardSets(int userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateFlashcardSet(int id, UpdateFlashcardSetCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
