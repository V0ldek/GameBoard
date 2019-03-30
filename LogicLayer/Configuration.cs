﻿using GameBoard.LogicLayer.Friends;
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
            services.AddScoped<IFriendsService, MockFriendsService>();
            DataLayer.Configuration.ConfigureServices(services);
        }

        public static IdentityBuilder AddDbContextStores(this IdentityBuilder builder) =>
            DataLayer.Configuration.AddDbContextStores(builder);
    }
}