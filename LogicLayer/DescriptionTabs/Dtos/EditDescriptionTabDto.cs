using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.DescriptionTabs.Dtos
{
    public class EditDescriptionTabDto
    {
        public int GameEventId { get; }

        [NotNull]
        public string Description { get; }

        public EditDescriptionTabDto(int gameEventId, [NotNull] string description)
        {
            GameEventId = gameEventId;
            Description = description;
        }
    }
}
