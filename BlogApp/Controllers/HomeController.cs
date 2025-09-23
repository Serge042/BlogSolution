using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BlogApp.Models;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public IActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View();
        }

        // Метод для тестирования страниц ошибок
        // Доступен по URL: /Home/TestError?code=404
        public IActionResult TestError(int code)
        {
            if (code == 404)
            {
                return NotFound();
            }
            else if (code == 403)
            {
                return Forbid();
            }
            else if (code == 500)
            {
                throw new Exception("Тестовая ошибка сервера");
            }

            return RedirectToAction("Index");
        }
    }
}