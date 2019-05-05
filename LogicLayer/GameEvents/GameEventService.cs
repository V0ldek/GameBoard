using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

namespace GameBoard.LogicLayer.GameEvents
{
    internal class GameEventService : IGameEventService
    {
        private readonly IGameBoardRepository _repository;

        public GameEventService(IGameBoardRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateGameEventAsync(CreateGameEventDto requestedGameEvent)
        {
            var creatorId = await _repository.GetUserIdByUserName(requestedGameEvent.CreatorUserName);
            var creatorParticipation = new GameEventParticipation
            {
                ParticipantId = creatorId,
                ParticipationStatus = ParticipationStatus.Creator
            };
            var gameEvent = new GameEvent
            {
                Name = requestedGameEvent.Name,
                MeetingTime = requestedGameEvent.MeetingTime,
                Place = requestedGameEvent.Place,
                Games = requestedGameEvent.Games
                    .Select(g => new Game { Name = g, GameStatus = GameStatus.ExistsOnTheList}).ToList(),
                Participations = new List<GameEventParticipation>()
            };

            gameEvent.Participations.Add(creatorParticipation);
            _repository.GameEvents.Add(gameEvent);

            await _repository.SaveChangesAsync();
        }

        public async Task EditGameEventAsync(EditGameEventDto editedEvent)
        {
            var gameEventAndGames = await _repository.GameEvents
                .Where(ge => ge.Id == editedEvent.Id)
                .Select(ge => new
                {
                    GameEvent = ge,
                    Games = ge.Games.Where(g => g.GameStatus == GameStatus.ExistsOnTheList)
                })
                .SingleAsync();

            var gameEvent = gameEventAndGames.GameEvent;
            var games = gameEventAndGames.Games;

            gameEvent.Name = editedEvent.Name;
            gameEvent.MeetingTime = editedEvent.MeetingTime;
            gameEvent.Place = editedEvent.Place;

            foreach (var game in games)
            {
                game.GameStatus = GameStatus.RemovedFromTheList; //Not sure if this is the right way to do this.
                _repository.Games.Update(game);
            }

            _repository.Games.AddRange(
                editedEvent.Games.Select(
                        g => new Game {Name = g, GameEventId = editedEvent.Id, GameStatus = GameStatus.ExistsOnTheList})
                    .ToList());

            await _repository.SaveChangesAsync();
        }

        private Task<List<GameEventListItemDto>> GetGameEventsWithSamePartitipationStatus(string userName, ParticipationStatus participationStatus)
        {
            var gameEvents = _repository
                .GetUserByUserName(userName)
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

        public async Task<GameEventListDto> GetAccessibleGameEventsAsync(string userName)
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

        public Task<GameEventDto> GetGameEventAsync(int gameEventId)
        {
            return  _repository.GameEvents
                .Where(ge => ge.Id == gameEventId)
                .Include(ge => ge.Games)
                .Include(ge => ge.Participations)
                .ThenInclude(p => p.Paticipant)
                .Select(ge => ge.ToGameEventDto()) // the conversion should be outside the query, but then the query won't be optimized (removed games will be included as well).
                .SingleAsync();
        }
    }
}
