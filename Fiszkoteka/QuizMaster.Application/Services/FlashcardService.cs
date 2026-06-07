using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Interfaces;
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



    }
}
