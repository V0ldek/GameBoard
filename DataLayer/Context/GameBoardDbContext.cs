using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GameBoard.DataLayer.Context
{
    internal sealed partial class GameBoardDbContext : IdentityDbContext<ApplicationUser>, IGameBoardRepository
    {
        public GameBoardDbContext(DbContextOptions<GameBoardDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.ConfigureWarnings(
                warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
    }
}