using JetBrains.Annotations;

namespace GameBoard.LogicLayer.DescriptionTabs.Dtos
{
    public class EditDescriptionTabDto
    {
        public int Id { get; }

        public int GameEventId { get; }

        public string Description { get; }

        public EditDescriptionTabDto(int id, int gameEventId, string description)
        {
            Id = id;
            GameEventId = gameEventId;
            Description = description;
        }
    }
}