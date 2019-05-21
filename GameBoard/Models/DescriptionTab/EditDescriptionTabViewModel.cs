using System.ComponentModel.DataAnnotations;
using GameBoard.LogicLayer.DescriptionTabs.Dtos;

namespace GameBoard.Models.DescriptionTab
{
    public class EditDescriptionTabViewModel
    {
        public int Id { get; set; }

        public int GameEventId { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        internal EditDescriptionTabDto ToDto() =>
            new EditDescriptionTabDto(Id, GameEventId, Description);
    }
}