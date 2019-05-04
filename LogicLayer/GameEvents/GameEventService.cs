﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEvents.Dtos;
using JetBrains.Annotations;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;


namespace GameBoard.LogicLayer.GameEvents
{
    class GameEventService : IGameEventService
    {
        private readonly IGameBoardRepository _repository;

        public GameEventService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateGameEventAsync([NotNull] CreateGameEventDto requestedGameEvent)
        {
            var creatorId = await _repository.GetUserIdByUserName(requestedGameEvent.CreatorUserName);
            var creatorParticipation = new GameEventParticipation()
            {
                ParticipantId = creatorId,
                ParticipationStatus = ParticipationStatus.Creator
            };
            var gameEvent = new GameEvent()
            {
                Name = requestedGameEvent.Name,
                MeetingTime = requestedGameEvent.MeetingTime,
                Place = requestedGameEvent.Place,
                Games = requestedGameEvent.Games.Select(g => new Game { Name = g }).ToList(),
                Participations = new List<GameEventParticipation>()
            };

            gameEvent.Participations.Add(creatorParticipation); // I don't know why, but it adds a participation with pendingInvitation status.
            _repository.GameEvents.Add(gameEvent);

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteGameEventAsync(int gameEventId)
        {
            var gameEvent = await _repository.GameEvents
                .Where(ge => ge.Id == gameEventId)
                .Include(ge => ge.Participations)
                .Include(ge => ge.Games)
                .SingleAsync();

            foreach (var game in gameEvent.Games)
            {
                _repository.Games.Remove(game);
            }
            foreach (var participation in gameEvent.Participations)
            {
                _repository.GameEventParticipations.Remove(participation);
            }
            _repository.GameEvents.Remove(gameEvent);

            await _repository.SaveChangesAsync();
        }

        public async Task EditGameEventAsync([NotNull] EditGameEventDto editedEvent)
        {
            var gameEvent = await _repository.GameEvents
                .Include(ge => ge.Games)
                .SingleAsync(ge => ge.Id == editedEvent.Id);

            gameEvent.Name = editedEvent.Name; // I think we should always overwrite the previous data.
            gameEvent.MeetingTime = editedEvent.MeetingTime; // Here too.
            gameEvent.Place = editedEvent.Place; // Here too.

            foreach (var game in gameEvent.Games)
            {
                _repository.Games.Remove(game); //not sure if it is correct to remove data. What is the other option then?
            }
            gameEvent.Games = editedEvent.Games.Select(g => new Game { Name = g }).ToList();

            await _repository.SaveChangesAsync();
        }

        private Task<List<GameEventListItemDto>> GetGameEventsWithSamePartitipationStatus([NotNull] string userName, ParticipationStatus participationStatus)
        {
            var normalizedUserName = userName.ToUpper();
            var user = _repository.ApplicationUsers
                .Where(u => u.NormalizedUserName == normalizedUserName);

            var gameEvents = user
                .Include(u => u.Participations)
                .SelectMany(u => u.Participations)
                .Where(p => p.ParticipationStatus == participationStatus)
                .Include(p => p.TakesPartIn)
                .Select(p => p.TakesPartIn);

            return gameEvents
                .Include(ge => ge.Participations)
                .ThenInclude(p => p.Paticipant)
                .Select(ge => ge.ToGameEventListItemDto()) // the conversion should be outside the query, but then the query won't be optimized. Well... I don't know if it will be optimized with what we have right now. We need also a filtered index on Creator.
                .ToListAsync();
        }

        public async Task<GameEventListDto> GetAccessibleGameEventsAsync([NotNull] string userName)
        {
            var creatorGameEvents =
                GetGameEventsWithSamePartitipationStatus(userName, ParticipationStatus.Creator);
            var invitees =
                GetGameEventsWithSamePartitipationStatus(userName, ParticipationStatus.PendingGuest);
            var participants =
                GetGameEventsWithSamePartitipationStatus(userName, ParticipationStatus.AcceptedGuest);
            // Perhaps it should be only one query with the succeding conversion.

            return new GameEventListDto(
                await creatorGameEvents,
                await invitees,
                await participants);
        }

        public async Task<GameEventDto> GetGameEventAsync(int gameEventId)
        {
            var gameEvent = await _repository.GameEvents
                .Where(ge => ge.Id == gameEventId)
                .Include(ge => ge.Games)
                .Include(ge => ge.Participations)
                .ThenInclude(p => p.Paticipant)
                .SingleAsync();

            return gameEvent.ToGameEventDto();
        }

        public async Task RejectGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName)
        {
            var userId = await _repository.GetUserIdByUserName(invitedUserName);
            var participation = await _repository.GameEventParticipations
                .SingleAsync(ge => ge.ParticipantId == userId 
                    && ge.Id == gameEventId 
                    && ge.ParticipationStatus != ParticipationStatus.RejectedGuest); //Two queries, code repetition.

            if (participation.ParticipationStatus != ParticipationStatus.PendingGuest)
            {
                throw new Exception(); //Here must come new Exception. I think it is unnecessary, a generic error will do.
            }
            participation.ParticipationStatus = ParticipationStatus.RejectedGuest;

            await _repository.SaveChangesAsync();
            
        }

        public async Task AcceptGameEventInvitationAsync(int gameEventId, [NotNull] string invitedUserName)
        {
            var userId = await _repository.GetUserIdByUserName(invitedUserName);
            var participation = await _repository.GameEventParticipations
                .SingleAsync(ge => ge.ParticipantId == userId
                    && ge.Id == gameEventId
                    && ge.ParticipationStatus != ParticipationStatus.RejectedGuest); //Two queries, code repetition.

            if (participation.ParticipationStatus != ParticipationStatus.PendingGuest)
            {
                throw new Exception(); //Here i must invent a name for new exception. I think it is unnecessary, a generic error will do.         
            }
            participation.ParticipationStatus = ParticipationStatus.AcceptedGuest;

            await _repository.SaveChangesAsync();

        }

        public async Task SendGameEventInvitationAsync(int gameEventId, [NotNull] string userName)
        {
            var userId = await _repository.GetUserIdByUserName(userName);
            var participation = await _repository.GameEventParticipations
                .SingleAsync(ge => ge.ParticipantId == userId
                    && ge.Id == gameEventId
                    && ge.ParticipationStatus != ParticipationStatus.RejectedGuest); //Two queries, code repetition. Why there is a ParticipationStatus != Rejected ?

            if (participation != null)
            {
                switch (participation.ParticipationStatus)
                {
                    case ParticipationStatus.PendingGuest:
                        throw new ApplicationException("You have already invited this user to this event.");
                    case ParticipationStatus.AcceptedGuest:
                        throw new ApplicationException("This user already participates in this event.");
                    default:
                        throw new ArgumentOutOfRangeException(); //maybe some message?

                }
            }

            var gameEventParticipation = new GameEventParticipation()
            {
                TakesPartInId = gameEventId,
                ParticipantId = userId,
                ParticipationStatus = ParticipationStatus.PendingGuest // Now it is not reundant
            };
            _repository.GameEventParticipations.Add(gameEventParticipation);

            await _repository.SaveChangesAsync();
        }
    }
}
