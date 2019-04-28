using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameBoard.Errors
{
    internal static class Error
    {
        public static IErrorHandlingWrapper FromController(Controller controller) =>
            new ErrorHandlingWrapper(controller);

        public static IErrorHandlingWrapper FromPage(PageModel pageModel) => new ErrorHandlingWrapper(pageModel);
    }
}