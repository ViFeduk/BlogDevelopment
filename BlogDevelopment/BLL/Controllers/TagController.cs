using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.Models.ViewModels;
using BlogDevelopment.Models.ViewModels.editModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogDevelopment.BLL.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // Получить все теги
        [HttpGet]
        [Route("/Tags")]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllAsync(); // Получаем список тегов через сервис
            return View(tags); // Передаем список тегов в представление
        }

       

        // Получить тег по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        // Создать новый тег
        [HttpPost]
        [Route("/Tag/Create")]
        public async Task<IActionResult> Create(TagViewModel tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }
            var newTag = new Tag
            {
                Name = tag.Name
            };
            await _tagService.CreateAsync(newTag);
            return RedirectToAction("GetAll");
        }
        [HttpGet]
        [Route("/Tags/add")]
       
        public async Task<IActionResult> AddTag()
        {
            
            return View(); // Передаем список тегов в представление
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound("Тег не найден.");
            }

            var model = new EditTagViewModel
            {
               Id = tag.Id,
                Name = tag.Name,
               
            };

            return View(model);
        }
        // Обновить тег
       
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Вернуть страницу с ошибками валидации
            }

            var tag = await _tagService.GetByIdAsync(model.Id);
            if (tag == null)
            {
                return NotFound("Тег не найден.");
            }

            tag.Name = model.Name;
            

            await _tagService.UpdateAsync(tag);

            return RedirectToAction("GetAll", "Tag"); // Перенаправить к списку тегов
        }

        // Удалить тег
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingTag = await _tagService.GetByIdAsync(id);
            if (existingTag == null)
            {
                return NotFound();
            }

            await _tagService.DeleteAsync(id);
            return NoContent();
        }
    }
}
