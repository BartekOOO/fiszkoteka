using QuizMaster.Contracts.Commands;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Application.Services
{
    public sealed class FlashcardSetService : IFlashcardSetService
    {
        private readonly IQuizMasterDbContext _context;

        public FlashcardSetService(IQuizMasterDbContext context)
        {
            _context = context;
        }

        public async Task<FlashcardSet> CreateFlashcardSet(CreateFlashCardSetCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<FlashcardSet> GetFlashcardSetDetails(int id, User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FlashcardSet>> GetFlashcardSets(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateFlashcardSet(int flashcardSetId, UpdateFlashcardSetCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
