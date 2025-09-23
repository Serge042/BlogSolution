using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Произошла необработанная ошибка");

            if (_env.IsDevelopment())
            {
                // В режиме разработки показываем детальную информацию об ошибке
                context.Result = new ContentResult
                {
                    Content = context.Exception.ToString(),
                    ContentType = "text/plain",
                    StatusCode = 500
                };
            }
            else
            {
                // В продакшене показываем общую страницу ошибки
                context.Result = new RedirectToActionResult("Error", "Home", null);
            }

            context.ExceptionHandled = true;
        }
    }
}