﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public sealed class CreateGameEventDto
    {
        [NotNull]
        public string CreatorUserName { get; }

        [NotNull]
        public string Name { get; }

        [CanBeNull]
        public DateTime? MeetingTime { get; } // It is not specified in the user story and I know we have a separate user story concerning this feature, but it won't work out most likely, so maybe we should put this in here for now.

        [CanBeNull]
        public string Place { get; }

        [NotNull]
        [ItemNotNull]
        public IEnumerable<string> Games;

        public CreateGameEventDto(
            [NotNull] string creatorUserName,
            [NotNull] string gameEventName,
            [CanBeNull] DateTime meetingTime,
            [CanBeNull] string place,
            [NotNull] [ItemNotNull] IEnumerable<string> games)
        {
            CreatorUserName = creatorUserName;
            Name = gameEventName;
            MeetingTime = meetingTime;
            Place = place;
            Games = games;
        }
    }
}