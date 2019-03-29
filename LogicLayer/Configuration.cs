using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GameBoard.LogicLayer
{
    public static class Configuration
    {
        public static void ConfigureDbContext(IServiceCollection services, string connectionString) =>
            DataLayer.Configuration.ConfigureDbContext(services, connectionString);

        public static void ConfigureServices(IServiceCollection services) =>  
            // TODO: Register implementations for IUserSearchService and IFriendsService
            DataLayer.Configuration.ConfigureServices(services);

        public static IdentityBuilder AddDbContextStores(this IdentityBuilder builder) =>
            DataLayer.Configuration.AddDbContextStores(builder);
    }
}