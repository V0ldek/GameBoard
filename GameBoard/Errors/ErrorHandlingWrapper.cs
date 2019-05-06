using System.Net;
using GameBoard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GameBoard.Errors
{
    internal sealed class ErrorHandlingWrapper : IErrorHandlingWrapper
    {
        private readonly LocalRedirectHelper _localRedirect;

        private readonly HttpResponse _response;
        private readonly ViewComponentHelper _viewComponent;

        internal ErrorHandlingWrapper(Controller controller)
        {
            _response = controller.Response;
            _localRedirect = controller.LocalRedirect;
            _viewComponent = controller.ViewComponent;
        }

        internal ErrorHandlingWrapper(PageModel pageModel)
        {
            _response = pageModel.Response;
            _localRedirect = pageModel.LocalRedirect;
            _viewComponent = pageModel.ViewComponent;
        }

        public LocalRedirectResult AccessDenied(string handlerLocalUri = "/Identity/Account/AccessDenied")
        {
            _response.StatusCode = (int) HttpStatusCode.Forbidden;
            return _localRedirect(handlerLocalUri);
        }

        public IActionResult Error(string title, string message, HttpStatusCode statusCode, ILogger logger = null)
        {
            var errorViewModel = new ErrorViewModel
            {
                Title = title,
                Message = message
            };

            _response.StatusCode = (int) statusCode;

            logger?.LogInformation(
                "The error view was called internally. " +
                $"Returning exit code {statusCode}",
                errorViewModel);

            return ErrorResult(errorViewModel);
        }

        public JsonResult ErrorJson(string title, string message, HttpStatusCode statusCode, ILogger logger = null)
        {
            var errorData = new
            {
                title,
                message
            };

            _response.StatusCode = (int) statusCode;

            logger?.LogInformation(
                $"Returning error JSON with status code {statusCode}.",
                errorData);

            return new JsonResult(errorData);
        }

        private ViewComponentResult ErrorResult(ErrorViewModel errorViewModel) =>
            _viewComponent("Error", errorViewModel);

        private delegate LocalRedirectResult LocalRedirectHelper(string localUri);

        private delegate ViewComponentResult ViewComponentHelper(string componentName, object data);
    }
}