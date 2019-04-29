using System;
using System.Collections.Generic;
using GameBoard.LogicLayer.UserSearch.Dtos;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public sealed class GameEventDto
    {
        public int GameEventId { get; }

        [NotNull]
        public string GameEventName { get; }

        [CanBeNull]
        public DateTime? MeetingTime { get; }

        [CanBeNull]
        public string Place { get; }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> Games;

        [NotNull]
        [ItemNotNull]
        public IEnumerable<UserDto> Users;

        public GameEventDto(
            int gameEventId,
            [NotNull] string gameEventName,
            [CanBeNull] DateTime? meetingTime,
            [CanBeNull] string place,
            [NotNull] [ItemNotNull] IEnumerable<string> games,
            [NotNull] [ItemNotNull] IEnumerable<UserDto> users)
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
