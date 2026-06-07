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
        Task<FlashcardSet> GetFlashcardSetDetails(int id, int userId, CancellationToken cancellationToken = default);
        Task<List<FlashcardSet>> GetFlashcardSets(int userId, CancellationToken cancellationToken = default);
        Task<FlashcardSet> CreateFlashcardSet(CreateFlashCardSetCommand command, CancellationToken cancellationToken = default);
        Task UpdateFlashcardSet(int id, UpdateFlashcardSetCommand command, CancellationToken cancellationToken);
        Task DeleteFlashcardSet(int  id, int userId , CancellationToken cancellationToken = default);
    }
}
