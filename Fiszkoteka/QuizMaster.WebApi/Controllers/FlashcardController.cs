using Microsoft.AspNetCore.Mvc;
using QuizMaster.Contracts.Interfaces;

namespace QuizMaster.WebApi.Controllers
{
    [ApiController]
    [Route("api/flashcard")]
    public sealed class FlashcardController : ControllerBase
    {
        private readonly IFlashcardService _flashcardService;

        public FlashcardController(IFlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
        }
    }
}
