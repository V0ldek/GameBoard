using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.UserSearchBox
{
    public class UserSearchBoxViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("UserSearchBox");
    }
}
