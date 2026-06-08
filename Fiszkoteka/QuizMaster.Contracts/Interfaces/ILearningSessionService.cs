using QuizMaster.Contracts.Commands.Learning;
using QuizMaster.Core.Dto;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Interfaces
{
    public interface ILearningSessionService
    {
        Task<LearningSessionDto> StartLearningSession(
            StartLearningSessionCommand command,
            CancellationToken cancellationToken = default);

        Task<LearningSessionDto> GetLearningSession(
            int sessionId,
            int userId,
            CancellationToken cancellationToken = default);

        Task<LearningFlashcardDto> GetNextFlashcard(
            int sessionId,
            int userId,
            CancellationToken cancellationToken = default);

        Task<AnswerFlashcardResultDto> AnswerFlashcard(
            int sessionId,
            AnswerFlashcardCommand command,
            CancellationToken cancellationToken = default);

        Task<LearningSessionDto> FinishLearningSession(
            int sessionId,
            int userId,
            CancellationToken cancellationToken = default);
    }
}
