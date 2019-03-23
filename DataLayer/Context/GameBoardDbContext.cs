using GameBoard.DataLayer.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Context
{
    internal sealed partial class GameBoardDbContext : IdentityDbContext, IGameBoardRepository
    {
        public GameBoardDbContext(DbContextOptions<GameBoardDbContext> options)
            : base(options)
        {
        }
    }
}
