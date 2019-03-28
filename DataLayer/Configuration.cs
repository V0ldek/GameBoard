using GameBoard.DataLayer.Context;
using GameBoard.DataLayer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameBoard.DataLayer
{
    public static class Configuration
    {
        public static void ConfigureDbContext(IServiceCollection services, string connectionString) =>
            services.AddDbContext<GameBoardDbContext>(
                options =>
                    options.UseNpgsql(connectionString));

        public static void ConfigureServices(IServiceCollection services) =>
            services.AddScoped<IGameBoardRepository, GameBoardDbContext>();

        public static IdentityBuilder AddDbContextStores(this IdentityBuilder builder) =>
            builder.AddEntityFrameworkStores<GameBoardDbContext>();
    }
}