using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Commands.Flashcards;
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
        private readonly IAuthService _authService;

        public FlashcardService(IQuizMasterDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public Task<Flashcard> CreateFlashcard(CreateFlashcardCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFlashcard(int id, int userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Flashcard>> GetFlashcards(int userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFlashcard(int id, UpdateFlashcardCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
