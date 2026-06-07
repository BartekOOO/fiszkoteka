using QuizMaster.Contracts.Commands.Flashcards;
using QuizMaster.Core.Models;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IFlashcardService
    {
        Task<List<Flashcard>> GetFlashcards(int userId, CancellationToken cancellationToken = default);
        Task<Flashcard> CreateFlashcard(CreateFlashcardCommand command, CancellationToken cancellationToken = default);
        Task UpdateFlashcard(int id, UpdateFlashcardCommand command, CancellationToken cancellationToken);
        Task DeleteFlashcard(int id, int userId, CancellationToken cancellationToken = default);
    }
}
