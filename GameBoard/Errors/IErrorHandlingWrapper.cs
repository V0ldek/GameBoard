using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameBoard.Errors
{
    internal interface IErrorHandlingWrapper
    {
        LocalRedirectResult AccessDenied(string handlerLocalUri = "/Identity/Account/AccessDenied");

        IActionResult Error(string title, string message, HttpStatusCode statusCode, ILogger logger = null);

        JsonResult ErrorJson(string title, string message, HttpStatusCode statusCode, ILogger logger = null);
    }
}