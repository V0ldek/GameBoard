using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public sealed class CreateGameEventDto
    {
        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> Games { get; }

        [NotNull]
        public string CreatorUserName { get; }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public DateTime? Date { get; }

        [CanBeNull]
        public string Place { get; }

        public CreateGameEventDto(
            [NotNull] string creatorUserName,
            [NotNull] string name,
            [CanBeNull] DateTime? date,
            [CanBeNull] string place,
            [NotNull] [ItemNotNull] IEnumerable<string> games)
        {
            CreatorUserName = creatorUserName;
            Name = name;
            Date = date;
            Place = place;
            Games = games;
        }
    }
}