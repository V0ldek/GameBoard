using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEvents.Dtos;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

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
            var creator = await _repository.ApplicationUsers.SingleAsync(
                ApplicationUser.UserNameEquals(requestedGameEvent.CreatorUserName));

            var creatorParticipation = new GameEventParticipation
            {
                ParticipantId = creator.Id,
                ParticipationStatus = ParticipationStatus.Creator
            };
            var gameEvent = new GameEvent
            {
                Name = requestedGameEvent.Name,
                Date = requestedGameEvent.MeetingTime,
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
            gameEvent.Date = editedEvent.MeetingTime;
            gameEvent.Place = editedEvent.Place;

            foreach (var game in games)
            {
                game.GameStatus = GameStatus.RemovedFromTheList;
                _repository.Games.Update(game);
            }

            await _repository.SaveChangesAsync();

            _repository.Games.AddRange(
                editedEvent.Games.Select(
                        g => new Game {Name = g, GameEventId = editedEvent.Id, GameStatus = GameStatus.ExistsOnTheList})
                    .ToList());

            await _repository.SaveChangesAsync();
        }

        public async Task<GameEventListDto> GetAccessibleGameEventsAsync(string userName)
        {
            var creatorGameEvents =
                GetGameEventsWithParticipationStatus(userName, ParticipationStatus.Creator);
            var invitees =
                GetGameEventsWithParticipationStatus(userName, ParticipationStatus.PendingGuest);
            var participants =
                GetGameEventsWithParticipationStatus(userName, ParticipationStatus.AcceptedGuest);

            return new GameEventListDto(
                await participants,
                await invitees,
                await creatorGameEvents);
        }

        private Task<List<GameEventListItemDto>> GetGameEventsWithParticipationStatus(
            [NotNull] string userName,
            ParticipationStatus participationStatus)
        {
            var gameEvents = _repository.ApplicationUsers
                .Where(ApplicationUser.UserNameEquals(userName))
                .Include(u => u.Participations)
                .SelectMany(u => u.Participations)
                .Where(p => p.ParticipationStatus == participationStatus)
                .Include(p => p.TakesPartIn)
                .Select(p => p.TakesPartIn);

            return gameEvents
                .Include(ge => ge.Participations)
                .ThenInclude(p => p.Participant)
                .Select(ge => ge.ToGameEventListItemDto())
                .ToListAsync();
        }

        public Task<GameEventDto> GetGameEventAsync(int gameEventId) =>
            _repository.GameEvents
                .Where(ge => ge.Id == gameEventId)
                .Include(ge => ge.Games)
                .Include(ge => ge.Participations)
                .ThenInclude(p => p.Participant)
                .Select(ge => ge.ToGameEventDto())
                .SingleOrDefaultAsync();
    }
}
