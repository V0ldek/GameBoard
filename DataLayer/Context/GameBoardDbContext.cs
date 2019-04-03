using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Context
{
    internal sealed partial class GameBoardDbContext : IdentityDbContext, IGameBoardRepository
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        public GameBoardDbContext(DbContextOptions<GameBoardDbContext> options)
            : base(options)
        {
        }
    }
}