using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Application.Services;
using QuizMaster.Contracts.Auth;
using QuizMaster.Contracts.Commands.Flashcards;
using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;
using QuizMaster.WebApi.Extensions;

namespace QuizMaster.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/flashcard")]
    public sealed class FlashcardController : ControllerBase
    {
        private readonly IFlashcardService _flashcardService;

        public FlashcardController(IFlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Flashcard), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateFlashcard(
           [FromBody] CreateFlashcardCommand command,
           CancellationToken cancellationToken)
        {
            command.UserId = this.GetCurrentUserId();

            var result = await _flashcardService.CreateFlashcard(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{flashcardSetId:int}")]
        [ProducesResponseType(typeof(List<Flashcard>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFlashcards(
            int flashCardSetId,
            CancellationToken cancellationToken)
        {
            var result = await _flashcardService.GetFlashcards(
                flashCardSetId,
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateFlashcard(
            int id,
            [FromBody] UpdateFlashcardCommand command,
            CancellationToken cancellationToken)
        {
            command.UserId = this.GetCurrentUserId();

            await _flashcardService.UpdateFlashcard(
                id,
                command,
                cancellationToken);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFlashcard(
            int id,
            CancellationToken cancellationToken)
        {
            await _flashcardService.DeleteFlashcard(
                id,
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok();
        }
    }
}
