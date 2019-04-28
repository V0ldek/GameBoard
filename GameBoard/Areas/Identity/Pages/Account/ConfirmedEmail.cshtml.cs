using System;
using System.Net;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameBoard.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Error.FromPage(this).Error("Error!", "Invalid URL.", HttpStatusCode.NotFound);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Error.FromPage(this).Error("Error!", "User specified in the URL does not exist.", HttpStatusCode.NotFound);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}'.");
            }

            return Page();
        }
    }
}