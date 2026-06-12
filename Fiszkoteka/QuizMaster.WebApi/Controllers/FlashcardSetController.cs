using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Contracts.Auth;
using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Contracts.Dto;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;
using QuizMaster.WebApi.Extensions;

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
        [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateFlashcardSet(
           [FromBody] CreateFlashcardSetCommand command,
           CancellationToken cancellationToken)
        {
            command.UserId = this.GetCurrentUserId();

            var result = await _flashcardSetService.CreateFlashcardSet(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FlashcardSetListItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFlashcardSets(
            CancellationToken cancellationToken)
        {
            var result = await _flashcardSetService.GetFlashcardSets(
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("public")]
        [ProducesResponseType(typeof(List<FlashcardSetListItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPublicFlashcardSets(
            [FromQuery] string? byUserName,
            [FromQuery] string? byCategory,
            CancellationToken cancellationToken)
        {
            var result = await _flashcardSetService.GetPublicFlashcardSets(
                byUserName,
                byCategory,
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(FlashcardSet), StatusCodes.Status200OK)]
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

        [HttpPut("{id:int}/Copy")]
        public async Task<IActionResult> CopyFlashcardSet(
            [FromQuery] int id,
            CancellationToken cancellationToken)
        {
            var result = await _flashcardSetService.CopyFlashcardSet(
                id,
                this.GetCurrentUserId(),
                cancellationToken);

            return Ok(result);
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
