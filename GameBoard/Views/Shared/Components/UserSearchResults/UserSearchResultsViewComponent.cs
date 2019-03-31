using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.UserSearch.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.UserSearchResult
{
    public class UserSearchResultsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<UserDto> searchResults) =>
            View("UserSearchResults", searchResults);
    }
}
