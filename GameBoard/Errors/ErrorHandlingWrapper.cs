using System.Net;
using GameBoard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace GameBoard.Errors
{
    internal sealed class ErrorHandlingWrapper : IErrorHandlingWrapper
    {
        private readonly LocalRedirectHelper _localRedirect;

        private readonly HttpResponse _response;
        private readonly ITempDataDictionary _tempData;
        private readonly ViewDataDictionary _viewData;

        internal ErrorHandlingWrapper(Controller controller)
        {
            _response = controller.Response;
            _viewData = controller.ViewData;
            _tempData = controller.TempData;
            _localRedirect = controller.LocalRedirect;
        }

        internal ErrorHandlingWrapper(PageModel pageModel)
        {
            _response = pageModel.Response;
            _viewData = pageModel.ViewData;
            _tempData = pageModel.TempData;
            _localRedirect = pageModel.LocalRedirect;
        }

        public LocalRedirectResult AccessDenied(string handlerLocalUri = "/Identity/Account/AccessDenied")
        {
            _response.StatusCode = (int) HttpStatusCode.Forbidden;
            return _localRedirect(handlerLocalUri);
        }

        public ViewResult Error(string title, string message, HttpStatusCode statusCode, ILogger logger = null)
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

            return View("Error", errorViewModel);
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

        private ViewResult View(string viewName, object model)
        {
            _viewData.Model = model;

            return new ViewResult
            {
                ViewName = viewName,
                ViewData = _viewData,
                TempData = _tempData
            };
        }

        private delegate LocalRedirectResult LocalRedirectHelper(string localUri);
    }
}