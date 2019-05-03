using GameBoard.Models.User;

namespace GameBoard.Models.GameEventList
{
    public sealed class GameEventListItemViewModel
    {
        public int Id { get; }

        public string Name { get; }

        public UserViewModel Creator { get; }

        public GameEventListItemViewModel(int id, string name, UserViewModel creator)
        {
            Id = id;
            Name = name;
            Creator = creator;
        }
    }
}
