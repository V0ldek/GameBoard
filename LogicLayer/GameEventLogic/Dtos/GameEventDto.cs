using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using GameBoard.LogicLayer.UserSearch.Dtos;

namespace GameBoard.LogicLayer.GameEventLogic.Dtos
{
    public sealed class GameEventDto
    {
        [NotNull]
        public string GameEventId { get; }

        [NotNull]
        public string GameEventName { get; }

        public DateTime MeetingTime { get; }

        public string Place { get; }

        [NotNull]
        public IEnumerable<string> Games;

        [NotNull]
        public IEnumerable<UserDto> Users;

        public GameEventDto(
            [NotNull] string gameEventId,
            [NotNull] string gameEventName,
            DateTime meetingTime,
            string place,
            [NotNull] IEnumerable<string> games,
            [NotNull] IEnumerable<UserDto> users)
        {
            GameEventId = gameEventId;
            GameEventName = gameEventName;
            MeetingTime = meetingTime;
            Place = place ?? string.Empty;
            Games = games;
            Users = users;
        }

    }
}
