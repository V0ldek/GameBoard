using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameBoard.Errors
{
    internal interface IErrorHandlingWrapper
    {
        LocalRedirectResult AccessDenied(string handlerLocalUri = "/Identity/Account/AccessDenied");

        ViewResult Error(string title, string message, HttpStatusCode statusCode, ILogger logger = null);

        JsonResult ErrorJson(string title, string message, HttpStatusCode statusCode, ILogger logger = null);
    }
}