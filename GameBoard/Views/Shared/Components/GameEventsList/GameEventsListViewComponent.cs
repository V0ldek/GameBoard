using System.Threading.Tasks;
using GameBoard.LogicLayer.GameEvents;
using GameBoard.Models.GameEventList;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.GameEventsList
{
    public class GameEventsListViewComponent : ViewComponent
    {
        private readonly IGameEventService _gameEventService;

        public GameEventsListViewComponent(IGameEventService gameEventService)
        {
            _gameEventService = gameEventService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("GameEventsListUnauthenticated");
            }

            var gameEventsList = await _gameEventService.GetAccessibleGameEventsAsync(User.Identity.Name);

            return View("GameEventsList", gameEventsList.ToViewModel());
        }
    }
}