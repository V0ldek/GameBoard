using System.Collections.Generic;
using GameBoard.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.UserSearchResults
{
    public class UserSearchResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<UserViewModel> searchResults) =>
            View("UserSearchResults", searchResults);
    }
}