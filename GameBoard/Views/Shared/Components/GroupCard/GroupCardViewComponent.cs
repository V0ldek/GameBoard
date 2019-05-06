using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameBoard.LogicLayer.Friends;
using GameBoard.LogicLayer.Groups;
using GameBoard.Models.Groups;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.GroupCard
{
    public class GroupCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(GroupViewModel group, bool miniature = false) =>
            View("GroupCard", group);
    }
}
