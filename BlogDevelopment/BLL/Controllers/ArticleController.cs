using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.Models.ViewModels;
using BlogDevelopment.Models.ViewModels.editModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NLog;

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
                Id = a.Id, // Передаем Id статьи
                Title = a.Title,
                Description = a.Description,
                Tags = a.PostTags?.Select(pt => new TagViewModel
                {
                    Id = pt.TagId,
                    Name = pt.Tag.Name
                }).ToList() ?? new List<TagViewModel>(),
                SelectedTagIds = a.PostTags?.Select(pt => pt.TagId).ToList() ?? new List<int>()
            }).ToList();

            return View(articleViewModels);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> ByAuthor(string userId)
        {
            var articles = await _articleService.GetByUserIdAsync(userId);
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            var articleViewModel = new ArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
                Tags = article.PostTags?.Select(pt => new TagViewModel
                {
                    Id = pt.TagId,
                    Name = pt.Tag.Name
                }).ToList() ?? new List<TagViewModel>(),
                 Comments = article.Comments?.Select(c => new CommentViewModel
                 {
                     UserName = c.User.UserName,
                     Text = c.Text,
                     CreatedAt = c.CreatedAt
                 }).ToList() ?? new List<CommentViewModel>()
            };

            return View(articleViewModel);
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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article == null || article.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            var allTags = await _tagService.GetAllAsync(); // Получаем все доступные теги
            var selectedTags = article.PostTags.Select(pt => pt.TagId).ToList(); // Получаем теги, связанные с этой статьей

            var model = new EditArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Description = article.Description,
                Tags = allTags.Select(tag => new TagViewModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsSelected = selectedTags.Contains(tag.Id) // Отмечаем тег, если он выбран
                }).ToList()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditArticleViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
               
                return View(model);
            }

            var existingArticle = await _articleService.GetByIdAsync(id);
            if (existingArticle == null || existingArticle.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            // Обновление данных статьи
            existingArticle.Title = model.Title;
            existingArticle.Description = model.Description;

            // Обновление тегов
            var selectedTagIds = model.SelectedTagIds;
            var currentTagIds = existingArticle.PostTags.Select(pt => pt.TagId).ToList();

            // Убираем теги, которые были сняты
            var tagsToRemove = existingArticle.PostTags.Where(pt => !selectedTagIds.Contains(pt.TagId)).ToList();
            foreach (var tag in tagsToRemove)
            {
                existingArticle.PostTags.Remove(tag);
            }

            // Добавляем новые выбранные теги
            var tagsToAdd = selectedTagIds.Where(tagId => !currentTagIds.Contains(tagId)).ToList();
            foreach (var tagId in tagsToAdd)
            {
                existingArticle.PostTags.Add(new PostTag { ArticleId = existingArticle.Id, TagId = tagId });
            }

            await _articleService.UpdateAsync(existingArticle);
            return RedirectToAction(nameof(Index)); // Перенаправить на список статей
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            if (article.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            await _articleService.DeleteAsync(id);
            return RedirectToAction("Profile", "Authentication"); // Перенаправление на профиль
        }

    }
}
