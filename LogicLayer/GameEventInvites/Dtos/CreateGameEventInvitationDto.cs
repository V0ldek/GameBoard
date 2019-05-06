﻿using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEventInvites.Dtos
{
    public sealed class CreateGameEventInvitationDto
    {
        [NotNull]
        public delegate string GameEventLinkGenerator([NotNull] string id);

        private readonly GameEventLinkGenerator _gameEventLinkGenerator;

        public int GameEventId { get; }

        [NotNull]
        public string UserNameTo { get; }

        public CreateGameEventInvitationDto(
            int gameEventId,
            [NotNull] string userNameTo,
            [NotNull] GameEventLinkGenerator gameEventLinkGenerator)
        {
            GameEventId = gameEventId;
            UserNameTo = userNameTo;
            _gameEventLinkGenerator = gameEventLinkGenerator;
        }

        [NotNull]
        public string GenerateGameEventLink([NotNull] string id) => _gameEventLinkGenerator(id);
    }
}