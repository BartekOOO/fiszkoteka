using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Contracts.Commands.FlashcardSet;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.WebApi.Extensions;
using System.Security.Claims;

namespace QuizMaster.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/flashcardset")]
    public sealed class FlashcardSetController : ControllerBase
    {
        private readonly IFlashcardSetService _flashcardSetService;

        public FlashcardSetController(IFlashcardSetService flashcardSetService)
        {
            _flashcardSetService = flashcardSetService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlashcardSet(
           [FromBody] CreateFlashCardSetCommand command,
           CancellationToken cancellationToken)
        {
            command.UserId = this.GetCurrentUserId();

            var result = await _flashcardSetService.CreateFlashcardSet(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFlashcardSets(CancellationToken cancellationToken)
        {
            var result = await _flashcardSetService.GetFlashcardSets(
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetFlashcardSetDetails(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _flashcardSetService.GetFlashcardSetDetails(
                id,
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateFlashcardSet(
            int id,
            [FromBody] UpdateFlashcardSetCommand command,
            CancellationToken cancellationToken)
        {
            command.UserId = this.GetCurrentUserId();

            await _flashcardSetService.UpdateFlashcardSet(
                id,
                command,
                cancellationToken);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFlashcardSet(
            int id,
            CancellationToken cancellationToken)
        {
            await _flashcardSetService.DeleteFlashcardSet(
                id,
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok();
        }
    }
}
