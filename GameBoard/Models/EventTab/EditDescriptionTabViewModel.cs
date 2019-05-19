using System.ComponentModel.DataAnnotations;
using GameBoard.LogicLayer.EventTabs.Dtos;

namespace GameBoard.Models.EventTab
{
    public class EditDescriptionTabViewModel
    {
        public int GameEventId { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        internal EditDescriptionTabDto ToDto() =>
            new EditDescriptionTabDto(GameEventId, Description);
    }
}