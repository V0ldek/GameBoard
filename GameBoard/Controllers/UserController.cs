using System.Linq;
using System.Threading.Tasks;
using GameBoard.LogicLayer.UserSearch;
using GameBoard.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserSearchService _userSearchService;

        public UserController(IUserSearchService userSearchService)
        {
            _userSearchService = userSearchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string input) =>
            ViewComponent(
                "UserSearchResults",
                (await _userSearchService.GetSearchCandidatesAsync(input)).Select(u => u.ToViewModel()));
    }
}