using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class GameEventListItemDto
    {
        public int Id { get; }

        [NotNull]
        public string Name { get; }

        [NotNull]
        public UserDto Creator { get; }

        internal GameEventListItemDto(
            int id,
            [NotNull] string name,
            [NotNull] UserDto creator)
        {
            Id = id;
            Name = name;
            Creator = creator;
        }
    }
}