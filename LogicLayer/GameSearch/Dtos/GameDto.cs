using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameSearch.Dtos
{
    public sealed class GameDto
    {
        [NotNull]
        public string Name { get; }

        public Int16 MinimalPlayerCount { get; }

        public Int16 MaximalPlayerCount { get; }

        public GameDto([NotNull] string name, Int16 minimalplayercount, Int16 maximalplayercount)
        {
            Name = name;
            MinimalPlayerCount = minimalplayercount;
            MaximalPlayerCount = maximalplayercount;
        }
    }
}
