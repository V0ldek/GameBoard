using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameBoard.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailInfoModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}