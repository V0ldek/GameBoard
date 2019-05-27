using System.Linq;
using GameBoard.Models.FriendSearch;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.FriendSearchResults
{
    public class FriendSearchResultsViewComponent : ViewComponent
    {
        private const int ResultsCap = 20;

        public IViewComponentResult Invoke(FriendSearchResultViewModel model)
        {
            var cappedResults = model.Users.Take(ResultsCap).ToList();

            return cappedResults.Any()
                ? View("FriendSearchResults", new FriendSearchResultViewModel(cappedResults, model.GroupId))
                : View("FriendSearchResultsEmpty", new FriendSearchResultViewModel(cappedResults, model.GroupId));
        }
    }
}