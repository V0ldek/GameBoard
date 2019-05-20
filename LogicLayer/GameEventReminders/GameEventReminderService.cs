using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.GameEventReminders.Notifications;
using GameBoard.LogicLayer.Notifications;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.GameEventReminders
{
    public class GameEventReminderService : IGameEventReminderService
    {
        private readonly INotificationService _notificationService;

        private readonly IGameBoardRepository _repository;

        public GameEventReminderService(IGameBoardRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task SendRemindersIfDateBetweenAsync(
            DateTime dateTimeFromInclusive,
            DateTime dateTimeToExclusive,
            GameEventLinkGenerator gameEventLinkGenerator)
        {
            var gameEvents = await GetEventsAndParticipationsWhereDateBetween(dateTimeFromInclusive, dateTimeToExclusive);
            var sendTasks = gameEvents.Select(g => SendReminder(g, gameEventLinkGenerator));
            await Task.WhenAll(sendTasks);
        }

        private Task<List<GameEvent>> GetEventsAndParticipationsWhereDateBetween(
            DateTime dateTimeFromInclusive,
            DateTime dateTimeToExclusive) =>
            _repository.GameEvents
                .Where(
                    g => g.Date.HasValue && g.Date.Value >= dateTimeFromInclusive && g.Date.Value < dateTimeToExclusive)
                .Include(g => g.Participations)
                .ThenInclude(g => g.Participant)
                .ToListAsync();

        private Task SendReminder(GameEvent gameEvent, GameEventLinkGenerator gameEventLinkGenerator)
        {
            Debug.Assert(gameEvent.Date.HasValue, "gameEvent.Date.HasValue");
            var users = ExtractParticipants(gameEvent);
            var notifications = users.Select(
                u => new EventReminderNotification(
                    gameEvent.Name,
                    u.UserName,
                    u.Email,
                    gameEvent.Date.Value,
                    gameEventLinkGenerator(gameEvent.Id.ToString())));

            return _notificationService.CreateNotificationBatch(notifications).SendAsync();
        }

        private static IEnumerable<ApplicationUser> ExtractParticipants(GameEvent gameEvent) =>
            gameEvent.Participations
                .Where(
                    p => p.ParticipationStatus == ParticipationStatus.AcceptedGuest ||
                        p.ParticipationStatus == ParticipationStatus.Creator)
                .Select(p => p.Participant).ToList();
    }
}