using System;
using System.Diagnostics;
using System.Net;
using GameBoard.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpContextAccessor httpContextAccessor, ILogger<HomeController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var context = _httpContextAccessor.HttpContext;
            var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var errorViewModel = new ErrorViewModel();

            switch (exceptionFeature?.Error)
            {
                case null:
                    Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    _logger.LogCritical(
                        "An unexpected error has occured and ExceptionHandlerPathFeature is unavailable. " +
                        $"Showing a generic message \"{errorViewModel.Message}\" to the user and returning 500",
                        Activity.Current);
                    break;
                case ApplicationException exception:
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    errorViewModel.Message = exception.Message;
                    _logger.LogError(
                        exception,
                        "An application error has occured. " +
                        $"Showing \"{exception.Message}\" as message to the user and returning 400.",
                        Activity.Current);
                    break;
                case Exception exception:
                    Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    _logger.LogCritical(
                        exception,
                        "An unexpected error has occured. " +
                        $"Showing a generic message \"{errorViewModel.Message}\" to the user and returning 500.",
                        Activity.Current);
                    break;
            }

            return View(errorViewModel);
        }
    }
}