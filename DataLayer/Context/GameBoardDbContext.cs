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
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        public Task<string> GetUserIdByUsername(string username)
        {
            var normalizedUserName = username.ToUpper();

            return ApplicationUsers
                .Where(u => u.NormalizedUserName == normalizedUserName)
                .Select(u => u.Id)
                .SingleAsync();
        }

        public GameBoardDbContext(DbContextOptions<GameBoardDbContext> options)
            : base(options)
        {
        }
    }
}