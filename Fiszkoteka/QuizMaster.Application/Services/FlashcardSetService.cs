using QuizMaster.Application.Interfaces;
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
