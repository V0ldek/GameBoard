using System;
using System.Threading.Tasks;

namespace GameBoard.LogicLayer.EventReminders
{
    public interface IEventReminderService
    {
        Task SendRemindersIfDateBetweenAsync(
            DateTime dateTimeFrom,
            DateTime dateTimeTo,
            EventReminderService.GameEventLinkGenerator gameEventLinkGenerator);
    }
}
