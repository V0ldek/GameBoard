using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace GameBoard.LogicLayer.GameEventReminders
{
    [NotNull]
    public delegate string GameEventLinkGenerator([NotNull] string id);

    public interface IGameEventReminderService
    {
        Task SendRemindersIfDateBetweenAsync(
            DateTime dateTimeFrom,
            DateTime dateTimeTo,
            GameEventLinkGenerator gameEventLinkGenerator);
    }
}