using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameBoard.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly IMailSender _mailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public InputModel Input { get; set; }

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IMailSender mailSender)
        {
            _userManager = userManager;
            _mailSender = mailSender;
        }

        private async void SendForgotPasswordEmail(ApplicationUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var email = Input.Email;
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                null,
                new { code, email },
                Request.Scheme);

            await _mailSender.SendPasswordResetAsync(Input.Email, HtmlEncoder.Default.Encode(callbackUrl));
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

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}