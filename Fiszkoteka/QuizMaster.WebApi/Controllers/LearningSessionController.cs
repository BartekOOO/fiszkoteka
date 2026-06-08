using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Contracts.Commands.Learning;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Dto;
using QuizMaster.WebApi.Extensions;

namespace QuizMaster.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/learning-session")]
    public sealed class LearningSessionController : ControllerBase
    {
        private readonly ILearningSessionService _learningSessionService;

        public LearningSessionController(ILearningSessionService learningSessionService)
        {
            _learningSessionService = learningSessionService;
        }

        [HttpPost("start")]
        [ProducesResponseType(typeof(LearningSessionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> StartLearningSession(
            [FromBody] StartLearningSessionCommand command,
            CancellationToken cancellationToken)
        {
            command.UserId = this.GetCurrentUserId();

            var result = await _learningSessionService.StartLearningSession(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LearningSessionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLearningSession(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _learningSessionService.GetLearningSession(
                id,
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}/next-flashcard")]
        [ProducesResponseType(typeof(LearningFlashcardDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNextFlashcard(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _learningSessionService.GetNextFlashcard(
                id,
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("{id:int}/answer")]
        [ProducesResponseType(typeof(AnswerFlashcardResultDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> AnswerFlashcard(
            int id,
            [FromBody] AnswerFlashcardCommand command,
            CancellationToken cancellationToken)
        {
            command.UserId = this.GetCurrentUserId();

            var result = await _learningSessionService.AnswerFlashcard(
                id,
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("{id:int}/finish")]
        [ProducesResponseType(typeof(LearningSessionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> FinishLearningSession(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _learningSessionService.FinishLearningSession(
                id,
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok(result);
        }
    }
}
