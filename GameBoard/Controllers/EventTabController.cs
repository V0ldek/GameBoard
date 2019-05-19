using System.Threading.Tasks;
using GameBoard.LogicLayer.EventTabs;
using GameBoard.Models.EventTab;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Controllers
{
    [Authorize]
    public class EventTabController : Controller
    {
        private readonly IEventTabService _eventTabService;

        public EventTabController(IEventTabService eventTabService)
        {
            _eventTabService = eventTabService;
        }

        [HttpGet]
        public IActionResult EditDescriptionTab(int id) => View("EditDescriptionTab", new EditDescriptionTabViewModel(){GameEventId = id});

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditDescriptionTab(EditDescriptionTabViewModel editDescriptionTabViewModel)
        {
            await _eventTabService.EditDescriptionTab(editDescriptionTabViewModel.ToDto());

            return RedirectToAction("GameEvent", "GameEvent", new {id = editDescriptionTabViewModel.GameEventId});
        }
    }
}