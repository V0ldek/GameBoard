using System.Threading.Tasks;
using GameBoard.LogicLayer.DescriptionTabs;
using GameBoard.Models.DescriptionTab;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Controllers
{
    [Authorize]
    public class DescriptionTabController : Controller
    {
        private readonly IDescriptionTabsService _eventTabService;

        public DescriptionTabController(IDescriptionTabsService eventTabService)
        {
            _eventTabService = eventTabService;
        }

        [HttpGet]
        public async Task<IActionResult> EditDescriptionTab(int descriptionTabId)
        {
            var description = await _eventTabService.GetDescriptionTabAsync(descriptionTabId);

            return View(
                "EditDescriptionTab",
                description.ToEditViewModel());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditDescriptionTab(EditDescriptionTabViewModel editDescriptionTabViewModel)
        {
            await _eventTabService.EditDescriptionTabAsync(editDescriptionTabViewModel.ToDto());

            return RedirectToAction("GameEvent", "GameEvent", new {id = editDescriptionTabViewModel.GameEventId});
        }
    }
}