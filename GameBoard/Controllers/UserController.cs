using System.Threading.Tasks;
using GameBoard.LogicLayer.UserSearch;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserSearchService _userSearchService;

        public UserController(IUserSearchService userSearchService)
        {
            _userSearchService = userSearchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string input) =>
            ViewComponent("UserSearchResults", await _userSearchService.GetSearchCandidatesAsync(input));
    }
}