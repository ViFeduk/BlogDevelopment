using BlogDevelopment.BLL.BusinesModels;
using BlogDevelopment.BLL.Services.Intarface;
using BlogDevelopment.Models.ViewModels;
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

        // Обновить тег
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tagService.UpdateAsync(tag);
            return NoContent();
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
