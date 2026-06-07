using Microsoft.AspNetCore.Mvc;
using QuizMaster.Contracts.Interfaces;

namespace QuizMaster.WebApi.Controllers
{
    [ApiController]
    [Route("api/flashcardset")]
    public sealed class FlashcardSetController : ControllerBase
    {
        private readonly IFlashcardSetService _flashcardSetService;

        public FlashcardSetController(IFlashcardSetService flashcardSetService)
        {
            _flashcardSetService = flashcardSetService;
        }
    }
}
