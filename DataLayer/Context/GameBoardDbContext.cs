using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Repositories;
using GameBoard.DataLayer.Transactions;
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

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<GameEvent> GameEvents { get; set; }
        public DbSet<GameEventParticipation> GameEventParticipations { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<DescriptionTab> DescriptionTabs { get; set; }

        private ITransaction _transaction;

        public ITransaction BeginTransaction()
        {
            if (_transaction == null || _transaction.TransactionFinished())
            {
                _transaction = new Transaction(Database.BeginTransaction());
            }

            return _transaction;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.ConfigureWarnings(
                warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
    }
}