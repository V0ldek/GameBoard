using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Transactions;
using GameBoard.Areas.Identity.Pages.Account.Notifications;
using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer.Configurations;
using GameBoard.LogicLayer.Groups;
using GameBoard.LogicLayer.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GameBoard.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly IGroupsService _groupsService;
        private readonly ILogger<RegisterModel> _logger;
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private GroupsConfiguration GroupsOptions { get; }

        [BindProperty]
        public InputModel Input { get; set; }

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            ILogger<RegisterModel> logger,
            IGroupsService groupsService,
            IOptions<GroupsConfiguration> groupsOptions,
            INotificationService notificationService)
        {
            _userManager = userManager;
            _logger = logger;
            _groupsService = groupsService;
            GroupsOptions = groupsOptions.Value;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser {UserName = Input.UserName, Email = Input.Email};

            IdentityResult result;
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await SendRegistrationEmailAsync(user);
                    await _groupsService.AddGroupAsync(user.UserName, GroupsOptions.AllFriendsGroupName);
                    transaction.Complete();

                    return RedirectToPage("/Account/ConfirmEmailInfo");
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        private async Task SendRegistrationEmailAsync(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmedEmail",
                null,
                new {userId = user.Id, code},
                Request.Scheme);

            var notification = new ConfirmEmailNotification(
                user.UserName,
                Input.Email,
                HtmlEncoder.Default.Encode(callbackUrl));
            await _notificationService.CreateNotificationBatch(notification).SendAsync();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(
                16,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 3)]
            [RegularExpression(@"^\w+$", ErrorMessage = "The {0} may contain only letters, numbers and underscores.")]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [StringLength(
                100,
                ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}