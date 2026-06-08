using Microsoft.EntityFrameworkCore;
using QuizMaster.Contracts.Models;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Application.Interfaces
{
    public interface IQuizMasterDbContext
    {
        DbSet<User> Users { get; }
        DbSet<FlashcardSet> FlashcardSets { get; }
        DbSet<Flashcard> Flashcards { get; }
        DbSet<UserFlashcardProgress> UserFlashcardProgresses { get; }
        DbSet<RevokedToken> RevokedTokens { get; }
        DbSet<Category> Categories { get; }
        DbSet<LearningSession> LearningSessions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
