using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Context
{
    internal sealed partial class GameBoardDbContext : IdentityDbContext<ApplicationUser>, IGameBoardRepository
    {
        public GameBoardDbContext(DbContextOptions<GameBoardDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }
        public DbSet<GameEventParticipation> GameEventParticipations { get; set; }
        public DbSet<Game> Games { get; set; }

        public IQueryable<ApplicationUser> GetUserByUserName(string userName)
        {
            var normalizedUserName = userName.ToUpper();

            return ApplicationUsers.Where(u => u.NormalizedUserName == normalizedUserName);
        }

        public Task<string> GetUserIdByUserName(string userName)
        {
            var normalizedUserName = userName.ToUpper();

            return ApplicationUsers
                .Where(u => u.NormalizedUserName == normalizedUserName)
                .Select(u => u.Id)
                .SingleAsync();
        }
    }
}