using System;
using System.ComponentModel.DataAnnotations;
using GameBoard.Utilities;

namespace GameBoard.Models.User
{
    public sealed class UserViewModel
    {
        public string Id { get; }

        [Display(Name = "Username")]
        public string UserName { get; }

        public Lazy<string> EmailHash { get; }

        public UserViewModel(string id, string userName, string email)
        {
            Id = id;
            UserName = userName;
            EmailHash = new Lazy<string>(() => GravatarEmailHasher.CalculateGravatarEmailHash(email));
        }
    }
}