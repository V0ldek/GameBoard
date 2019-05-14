using GameBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameBoard.Views.Shared.Components.Error
{
    public class ErrorViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ErrorViewModel errorViewModel) => View(errorViewModel);
    }
}