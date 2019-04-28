using System;
using System.Collections.Generic;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public sealed class GameEventDto
    {
        [NotNull]
        public int GameEventId { get; }

        [NotNull]
        public string GameEventName { get; }

        public DateTime MeetingTime { get; }

        public string Place { get; }

        [NotNull]
        public IEnumerable<string> Games;

        [NotNull]
        public IEnumerable<UserDto> Users;

        public GameEventDto(
            [NotNull] int gameEventId,
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
