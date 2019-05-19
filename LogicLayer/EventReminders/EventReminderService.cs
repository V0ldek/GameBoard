using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using GameBoard.DataLayer.Repositories;
using GameBoard.LogicLayer.EventReminders.Notifications;
using GameBoard.LogicLayer.Notifications;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.LogicLayer.EventReminders
{
    public class EventReminderService : IEventReminderService
    {
        [NotNull]
        public delegate string GameEventLinkGenerator([NotNull] string id);

        private readonly IGameBoardRepository _repository;

        private readonly INotificationService _notificationService;

        public EventReminderService(IGameBoardRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        public async Task SendRemindersIfDateBetweenAsync(
            DateTime dateTimeFrom,
            DateTime dateTimeTo,
            GameEventLinkGenerator gameEventLinkGenerator)
        {
            var gameEvents = await GetEventsAndParticipationsWhereDateBetween(dateTimeFrom, dateTimeTo);
            var sendTasks = new List<Task>();

            foreach (var gameEvent in gameEvents)
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
                sendTasks.Add(_notificationService.CreateNotificationBatch(notifications).SendAsync());
            }

            await Task.WhenAll(sendTasks);
        }

        private Task<List<GameEvent>> GetEventsAndParticipationsWhereDateBetween(
            DateTime dateTimeFrom,
            DateTime dateTimeTo) =>
            _repository.GameEvents.Where(
                    g => g.Date.HasValue && g.Date.Value >= dateTimeFrom && g.Date.Value < dateTimeTo)
                .Include(g => g.Participations)
                .ThenInclude(g => g.Participant)
                .ToListAsync();

        private IEnumerable<ApplicationUser> ExtractParticipants(GameEvent gameEvent) =>
            gameEvent.Participations.Where(
                p => p.ParticipationStatus == ParticipationStatus.AcceptedGuest ||
                    p.ParticipationStatus == ParticipationStatus.Creator).Select(p => p.Participant).ToList();
    }
}