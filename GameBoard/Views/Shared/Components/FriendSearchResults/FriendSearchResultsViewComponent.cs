using System.Collections.Generic;
using System.Linq;
using GameBoard.Models.Groups;
using GameBoard.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.FriendSearchResults
{
    public class FriendSearchResultsViewComponent : ViewComponent
    {
        private const int ResultsCap = 20;

        public IViewComponentResult Invoke(GroupViewModel searchResults)
        {
            var cappedResults = searchResults.Users.Take(ResultsCap).ToList();

            return cappedResults.Any() ? View("FriendSearchResults", new GroupViewModel(searchResults.GroupId, searchResults.GroupName, cappedResults)) : View("FriendSearchResultsEmpty");
        }
    }
}