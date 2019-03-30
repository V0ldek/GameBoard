using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
            EmailHash = new Lazy<string>(() => Utilities.CalculateGravatarEmailHash(email));
        }
    }
}
