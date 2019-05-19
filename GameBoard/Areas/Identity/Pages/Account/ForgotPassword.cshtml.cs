using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.Notifications;
using GameBoard.LogicLayer.Notifications.Old;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameBoard.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly IOldMailSender _oldMailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public InputModel Input { get; set; }

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IOldMailSender oldMailSender)
        {
            _userManager = userManager;
            _oldMailSender = oldMailSender;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            SendForgotPasswordEmail(user);

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

        private async void SendForgotPasswordEmail(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var email = Input.Email;
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                null,
                new {code, email},
                Request.Scheme);

            await _oldMailSender.SendPasswordResetAsync(Input.Email, HtmlEncoder.Default.Encode(callbackUrl));
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}