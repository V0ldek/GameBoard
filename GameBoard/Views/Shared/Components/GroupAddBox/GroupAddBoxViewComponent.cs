using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.GroupAddBox
{
    public class GroupAddBoxViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("GroupAddBox");
    }
}
