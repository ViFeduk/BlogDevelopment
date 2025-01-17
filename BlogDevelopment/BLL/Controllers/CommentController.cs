using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogDevelopment.BLL.Controllers
{
    
  
    public class CommentController : Controller
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
        [HttpGet]
        public async Task<IActionResult> GetComments(int articleId)
        {
            var comments = await _commentService.GetAllAsync();
            var articleComments = comments
                .Where(c => c.ArticleId == articleId)
                .Select(c => new CommentViewModel
                {
                    UserName = c.User.UserName,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt
                })
                .ToList();

            // Если articleComments пуст, передаем пустой список вместо null
            return PartialView("_CommentsList", articleComments ?? new List<CommentViewModel>());
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
        [Route("AddComment")]
        public async Task<IActionResult> AddComment(int articleId, string commentText)
        {
            // Получаем текущего пользователя
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Используем Identity для получения UserId

            // Проверяем, что текст комментария не пуст
            if (string.IsNullOrEmpty(commentText))
            {
                return BadRequest("Текст комментария не может быть пустым.");
            }

            // Создаем новый комментарий
            var comment = new Comment
            {
                Text = commentText,
                UserId = userId,
                ArticleId = articleId, // Здесь связываем комментарий с конкретной статьей
                CreatedAt = DateTime.Now
            };

            // Сохраняем комментарий
            await _commentService.CreateAsync(comment);

            // После добавления комментария можно перенаправить обратно на страницу статьи
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
