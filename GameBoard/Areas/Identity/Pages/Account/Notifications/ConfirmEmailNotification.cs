using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.Notifications.HtmlTemplateNotifications.Notifications;

namespace GameBoard.Areas.Identity.Pages.Account.Notifications
{
    internal class ConfirmEmailNotification : LinkAndTextNotification
    {
        public ConfirmEmailNotification(string userName, string userEmail, string url) : base(
            "Confirm your email",
            new[] {userEmail},
            url,
            "Confirm email",
            $"Hello, {userName}!",
            "Thank you for joining GameBoard! Before you start using our website, you need to confirm your email by clicking on the link below.",
            "If you have not registered an account on GameBoard recently, please ignore this email.")
        {
        }
    }
}