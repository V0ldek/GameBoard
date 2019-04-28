﻿using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections.Generic;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.DataLayer.Entities;

namespace GameBoard.LogicLayer.GameEvents
{
    // Perhaps this interface should be split into three separate interfaces.
    public interface IGameEventService
    {
        // Throws CreateGameEventException when new GameEvent could not be created (what can go wrong?)
        Task CreateGameEventAsync([NotNull] CreateGameEventDto requestedGameEvent, [NotNull] IEnumerable<string> games);
        
        // Throws DeleteGameEventException when GameEvent could not be deleted (Event with such id doesn't exist, but I don't know if that is actually possible)
        Task DeleteGameEventAsync([NotNull] string gameEventId);

        Task EditGameEventAsync( [NotNull] EditGameEventDto editedEvent, [NotNull] IEnumerable<string> newGames);

        // Returns Task with null if game event does not exist.
        [ItemCanBeNull]
        Task<GameEventDto> GetGameEventAsync([NotNull] string gameEventId);

        Task<IEnumerable<GameEventDto>> GetAccessibleGameEventsAsync([NotNull] string userId);

        Task<IEnumerable<GameEventDto>> GetPendingGameEventsAsync([NotNull] string userId);

        Task<GameEventPermission> GetGameEventPermissionByUserAsync([NotNull] string gameEventId, [NotNull] string userId);

        Task SendGameEventInvitationAsync([NotNull] string gameEventId, [NotNull] string userId);

        Task AcceptGameEventInvitationAsync([NotNull] string gameEventId);

        Task RejectGameEventInvitationAsync([NotNull] string gameEventId);
    }
}
