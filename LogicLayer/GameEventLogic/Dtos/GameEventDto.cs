using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using GameBoard.LogicLayer.GameSearch.Dtos;

namespace GameBoard.LogicLayer.GameEventLogic.Dtos
{
    public sealed class GameEventDto
    {
        [NotNull]
        public string Id { get; }

        [NotNull]
        public string UserId { get; }

        [NotNull]
        public DateTime MeetingTime { get; }

        public ICollection<GameDto> Games;

        //public ICollection<GameEventInvitationDto> Invitations; 
    }
}
