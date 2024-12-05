using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogDevelopment.BLL.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticleController(IArticleService articleService, UserManager<ApplicationUser> userManager)
        {
            _articleService = articleService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllAsync();
            return View(articles);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> ByAuthor(string userId)
        {
            var articles = await _articleService.GetByUserIdAsync(userId);
            return View(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            article.UserId = _userManager.GetUserId(User); // Привязываем статью к текущему пользователю
            await _articleService.CreateAsync(article);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article == null || article.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }
            return View(article);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(article);
            }

            var existingArticle = await _articleService.GetByIdAsync(id);
            if (existingArticle == null || existingArticle.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            existingArticle.Title = article.Title;
            existingArticle.Description = article.Description;

            await _articleService.UpdateAsync(existingArticle);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article == null || article.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            await _articleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
