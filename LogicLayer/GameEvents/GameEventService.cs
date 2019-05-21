using System;
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
    internal sealed class GameEventService : IGameEventService
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
                Date = requestedGameEvent.Date,
                Place = requestedGameEvent.Place,
                Games = requestedGameEvent.Games
                    .Select((game, index) => new Game {Name = game, PositionOnTheList = index}).ToList(),
                Participations = new List<GameEventParticipation>(),
                Description = new DescriptionTab()
            };

            gameEvent.Participations.Add(creatorParticipation);
            _repository.GameEvents.Add(gameEvent);

            await _repository.SaveChangesAsync();
        }

        public async Task EditGameEventAsync(EditGameEventDto editedEvent)
        {
            var transaction = _repository.BeginTransaction();

            await EditSingleGameEventProperties(editedEvent.Id, editedEvent.Name, editedEvent.Date, editedEvent.Place);

            var games = await GetGames(editedEvent.Id);

            await EditGames(editedEvent.Id, games, editedEvent.Games);

            transaction.Commit();
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
                .Include(ge => ge.Description)
                .Select(ge => ge.ToGameEventDto())
                .SingleOrDefaultAsync();

        private async Task EditSingleGameEventProperties(
            int gameEventId,
            [NotNull] string name,
            [CanBeNull] DateTime? date,
            [CanBeNull] string place)
        {
            var gameEvent = await _repository.GameEvents
                .SingleAsync(ge => ge.Id == gameEventId);

            gameEvent.Name = name;
            gameEvent.Date = date;
            gameEvent.Place = place;
            await _repository.SaveChangesAsync();
        }

        private Task<List<Game>> GetGames(int gameEventId) =>
            _repository.GameEvents
                .Where(ge => ge.Id == gameEventId)
                .Include(ge => ge.Games)
                .SelectMany(ge => ge.Games)
                .Where(g => g.PositionOnTheList != null)
                .ToListAsync();

        private async Task EditGames(
            int gameEventId,
            ICollection<Game> previousGamesList,
            IEnumerable<string> newGamesList)
        {
            foreach (var game in previousGamesList)
            {
                game.PositionOnTheList = null;
            }

            await _repository.SaveChangesAsync();

            foreach (var (name, index) in newGamesList.Select((g, i) => (g, i)))
            {
                var game = previousGamesList.FirstOrDefault(g => g.Name == name);

                if (game == null)
                {
                    _repository.Games.Add(
                        new Game {Name = name, GameEventId = gameEventId, PositionOnTheList = index});
                }
                else
                {
                    previousGamesList.Remove(game);
                    game.PositionOnTheList = index;
                }
            }

            await _repository.SaveChangesAsync();
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
    }
}