using GameBoard.LogicLayer.DescriptionTabs;
using GameBoard.LogicLayer.Friends;
using GameBoard.LogicLayer.GameEventParticipations;
using GameBoard.LogicLayer.GameEvents;
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
            services.AddScoped<IUserSearchService, UserSearchService>();
            services.AddScoped<IFriendsService, FriendsService>();
            services.AddScoped<IGameEventService, GameEventService>();
            services.AddScoped<IGameEventParticipationService, GameEventParticipationService>();
            services.AddScoped<IDescriptionTabsService, DescriptionTabsService>();
        }

        public static IdentityBuilder AddDbContextStores(this IdentityBuilder builder) =>
            DataLayer.Configuration.AddDbContextStores(builder);
    }
}