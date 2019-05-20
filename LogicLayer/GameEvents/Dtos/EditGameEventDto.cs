using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public class EditGameEventDto
    {
        [NotNull] [ItemNotNull]
        public IEnumerable<string> Games { get; }

        public int Id { get; }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public DateTime? Date { get; }

        [CanBeNull]
        public string Place { get; }

        public EditGameEventDto(
            int id,
            [NotNull] string name,
            [CanBeNull] DateTime? date,
            [CanBeNull] string place,
            [NotNull] [ItemNotNull] IEnumerable<string> games)
        {
            Id = id;
            Name = name;
            Date = date;
            Place = place;
            Games = games;
        }
    }
}