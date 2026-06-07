using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;
using QuizMaster.WebApi.Extensions;

namespace QuizMaster.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/category")]
    public sealed class CategoryController : ControllerBase
    {
        private readonly IQuizMasterDbContext _quizMasterDbContext;

        public CategoryController(IQuizMasterDbContext quizMasterDbContext)
        {
            _quizMasterDbContext = quizMasterDbContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var result = _quizMasterDbContext.Categories.ToList();
            return Ok(result);
        }
    }
}
