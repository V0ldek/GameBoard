using System.Net;
using GameBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameBoard.Controllers
{
    internal static class ControllerExtensions
    {
        public static LocalRedirectResult AccessDenied(
            this Controller controller,
            string handlerLocalUri = "/Identity/Account/AccessDenied")
        {
            controller.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            return controller.LocalRedirect(handlerLocalUri);
        }

        public static ViewResult Error(
            this Controller controller,
            string title,
            string message,
            HttpStatusCode statusCode,
            ILogger logger = null)
        {
            var errorViewModel = new ErrorViewModel
            {
                Title = title,
                Message = message
            };

            controller.Response.StatusCode = (int) statusCode;

            logger?.LogInformation(
                "The error view was called internally. " +
                $"Returning exit code {statusCode}",
                errorViewModel);

            return controller.View("Error", errorViewModel);
        }

        public static JsonResult ErrorJson(
            this Controller controller,
            string title,
            string message,
            HttpStatusCode statusCode,
            ILogger logger = null)
        {
            var errorData = new
            {
                title,
                message
            };

            controller.Response.StatusCode = (int) statusCode;

            logger?.LogInformation(
                $"Returning error JSON with status code {statusCode}.",
                errorData);

            return controller.Json(errorData);
        }
    }
}