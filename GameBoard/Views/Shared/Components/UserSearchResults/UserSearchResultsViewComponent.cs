using System.Collections.Generic;
using System.Linq;
using GameBoard.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.UserSearchResults
{
    public class UserSearchResultsViewComponent : ViewComponent
    {
        private const int ResultsCap = 20;

        public IViewComponentResult Invoke(IEnumerable<UserViewModel> searchResults)
        {
            var cappedResults = searchResults.Take(ResultsCap).ToList();

            return cappedResults.Any() ? View("UserSearchResults", cappedResults) : View("UserSearchResultsEmpty");
        }
    }
}