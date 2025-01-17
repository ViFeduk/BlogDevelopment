using Microsoft.AspNetCore.Mvc;

namespace BlogDevelopment.BLL.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View("Error/InternalServerError"); // Возвращаем представление для ошибки 500
        }

        // Для ошибки 404
        [Route("Error/404")]
        public IActionResult NotFoundError()
        {
            return View("Error/NotFound"); // Возвращаем представление для ошибки 404
        }

        // Для ошибки 403
        [Route("Error/403")]
        public IActionResult AccessDenied()
        {
            return View("Error/AccessDenied"); // Возвращаем представление для ошибки 403
        }
    }
}
