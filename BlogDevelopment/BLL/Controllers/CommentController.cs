using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services.Intarface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogDevelopment.BLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ICommentService commentService, UserManager<ApplicationUser> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        // Получить все комментарии
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentService.GetAllAsync();
            return Ok(comments);
        }

        // Получить комментарий по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // Создать новый комментарий
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int articleId, string commentText)
        {
            if (string.IsNullOrEmpty(commentText))
            {
                ModelState.AddModelError(string.Empty, "Комментарий не может быть пустым.");
                return RedirectToAction("Details", "Article", new { id = articleId });
            }

            var user = await _userManager.GetUserAsync(User);

            var comment = new Comment
            {
                Text = commentText,
                UserId = user.Id,
                ArticleId = articleId,
                CreatedAt = DateTime.UtcNow
            };

            await _commentService.AddCommentAsync(comment);

            return RedirectToAction("Details", "Article", new { id = articleId });
        }

        // Обновить комментарий
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _commentService.UpdateAsync(comment);
            return NoContent();
        }

        // Удалить комментарий
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingComment = await _commentService.GetByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            await _commentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
