using QuizMaster.Contracts.Commands;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IFlashcardSetService
    {
        Task<FlashcardSet> GetFlashcardSetDetails(int id, User user, CancellationToken cancellationToken = default);
        Task<List<FlashcardSet>> GetFlashcardSets(User user, CancellationToken cancellationToken = default);
        Task<FlashcardSet> CreateFlashcardSet(CreateFlashCardSetCommand command, CancellationToken cancellationToken = default);
        Task UpdateFlashcardSet(int flashcardSetId, UpdateFlashcardSetCommand command, CancellationToken cancellationToken);
    }
}
