using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.Models.ViewModels;
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
        private readonly ITagService _tagService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArticleController(IArticleService articleService, UserManager<ApplicationUser> userManager, ITagService tagService)
        {
            _articleService = articleService;
            _userManager = userManager;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllAsync();
            var articleViewModels = articles.Select(a => new ArticleViewModel
            {
                Title = a.Title,
                Description = a.Description,
                Tags = a.PostTags?.Select(pt => new TagViewModel
                {
                    Id = pt.TagId,
                    Name = pt.Tag.Name, // Преобразуем теги в TagViewModel
                    IsSelected = false // Вы можете изменить логику выбора тега, если это необходимо
                }).ToList() ?? new List<TagViewModel>(), // Если PostTags == null, создаём пустой список
                SelectedTagIds = a.PostTags?.Select(pt => pt.TagId).ToList() ?? new List<int>() // Если PostTags == null, создаём пустой список
            }).ToList();
            return View(articleViewModels);
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
        public async Task<IActionResult> Create()
        {
            var tags = await _tagService.GetAllAsync();

            // Создаем модель для представления
            var model = new ArticleViewModel
            {
                Tags = tags.Select(tag => new TagViewModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsSelected = false
                }).ToList()
            };

            return View(model); // Передаем моде
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel article)
        {
            if (!ModelState.IsValid)
            {
                return View(article);
            }
            var tags = await _tagService.GetAllAsync();
            var articleEntity = new Article
            {
                Title = article.Title,
                Description = article.Description,
                UserId = _userManager.GetUserId(User),
                PostTags = article.SelectedTagIds
            .Select(tagId => new PostTag
            {
                TagId = tagId, // Привязываем к существующему тегу через TagId
                Tag = tags.FirstOrDefault(t => t.Id == tagId) // Загружаем Tag через TagId
            })
            .ToList()
            };

            articleEntity.UserId = _userManager.GetUserId(User); // Привязываем статью к текущему пользователю
            await _articleService.CreateAsync(articleEntity);
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
