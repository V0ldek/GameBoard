using GameBoard.Models.Groups;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.GroupCard
{
    public class GroupCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(GroupViewModel group) =>
            View("GroupCard", group);
    }
}
