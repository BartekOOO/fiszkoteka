using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Contracts.Dto;
using QuizMaster.Core.Models;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IFlashcardSetService
    {
        Task<FlashcardSet> GetFlashcardSetDetails(int id, int userId, CancellationToken cancellationToken = default);
        Task<List<FlashcardSetListItemDto>> GetFlashcardSets(int userId, CancellationToken cancellationToken = default);
        Task<List<FlashcardSetListItemDto>> GetPublicFlashcardSets(string? userName, string? categoryName, int userId, CancellationToken cancellationToken);
        Task<FlashcardSet> CreateFlashcardSet(CreateFlashcardSetCommand command, CancellationToken cancellationToken = default);
        Task UpdateFlashcardSet(int id, UpdateFlashcardSetCommand command, CancellationToken cancellationToken);
        Task DeleteFlashcardSet(int  id, int userId , CancellationToken cancellationToken = default);
        Task<CopiedFlashcardSetDto> CopyFlashcardSet(int id, int userId, CancellationToken cancellationToken);
    }
}
