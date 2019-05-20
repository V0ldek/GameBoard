using GameBoard.LogicLayer.Friends;
using GameBoard.LogicLayer.GameEventParticipations;
using GameBoard.LogicLayer.GameEventReminders;
using GameBoard.LogicLayer.GameEvents;
using GameBoard.LogicLayer.Notifications;
using GameBoard.LogicLayer.Notifications.SendGrid;
using GameBoard.LogicLayer.UserSearch;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GameBoard.LogicLayer
{
    public static class Configuration
    {
        public static void ConfigureDbContext(IServiceCollection services, string connectionString) =>
            DataLayer.Configuration.ConfigureDbContext(services, connectionString);

        public static void ConfigureServices(IServiceCollection services)
        {
            DataLayer.Configuration.ConfigureServices(services);
            services.AddTransient<INotificationService, SendGridNotificationService>();
            services.AddScoped<IGameEventReminderService, GameEventReminderService>();
            services.AddScoped<IUserSearchService, UserSearchService>();
            services.AddScoped<IFriendsService, FriendsService>();
            services.AddScoped<IGameEventService, GameEventService>();
            services.AddScoped<IGameEventParticipationService, GameEventParticipationService>();
        }

        public static IdentityBuilder AddDbContextStores(this IdentityBuilder builder) =>
            DataLayer.Configuration.AddDbContextStores(builder);
    }
}