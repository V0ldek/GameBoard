using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.Models.User;

namespace GameBoard.Models.GameEventList
{
    public sealed class GameEventListItemViewModel
    {
        public int GameEventId { get; }

        public string GameEventName { get; }

        public UserViewModel Creator { get; }

        public GameEventListItemViewModel(int gameEventId, string gameEventName, UserViewModel creator)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            Creator = creator;
        }
    }
}
