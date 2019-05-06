using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEvents.Dtos;
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
                    .Select((game, index) => new Game {Name = game, PositionOnTheList = index}).ToList(),
                Participations = new List<GameEventParticipation>()
            };

            gameEvent.Participations.Add(creatorParticipation);
            _repository.GameEvents.Add(gameEvent);

            await _repository.SaveChangesAsync();
        }

        public async Task EditGameEventAsync(EditGameEventDto editedEvent)
        {
            var gameEvent = await _repository.GameEvents
                .SingleAsync(ge => ge.Id == editedEvent.Id);

            var games = await _repository.GameEvents
                .Where(ge => ge.Id == editedEvent.Id)
                .Include(ge => ge.Games)
                .SelectMany(ge => ge.Games)
                .Where(g => g.PositionOnTheList != null)
                .ToListAsync();

            gameEvent.Name = editedEvent.Name;
            gameEvent.Date = editedEvent.MeetingTime;
            gameEvent.Place = editedEvent.Place;

            foreach (var game in games)
            {
                game.PositionOnTheList = null;
            }

            await _repository.SaveChangesAsync();

            foreach (var it in editedEvent.Games.Select((gameName, index) => new {gameName, index}))
            {
                var game = games.FirstOrDefault(g => g.Name == it.gameName);

                if (game == null)
                {
                    _repository.Games.Add(
                        new Game {Name = it.gameName, GameEventId = gameEvent.Id, PositionOnTheList = it.index});
                }
                else
                {
                    games.Remove(game);
                    game.PositionOnTheList = it.index;
                }
            }

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

        public Task<GameEventDto> GetGameEventAsync(int gameEventId) =>
            _repository.GameEvents
                .Where(ge => ge.Id == gameEventId)
                .Include(ge => ge.Games)
                .Include(ge => ge.Participations)
                .ThenInclude(p => p.Participant)
                .Select(ge => ge.ToGameEventDto())
                .SingleOrDefaultAsync();

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
    }
}