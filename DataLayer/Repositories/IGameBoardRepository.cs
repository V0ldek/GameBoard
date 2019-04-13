using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Repositories
{
    public interface IGameBoardRepository
    {
        DbSet<ApplicationUser> ApplicationUsers { get; set; }

        DbSet<Game> Games { get; set; }

        DbSet<GameEvent> GameEvents { get; set; }

        Task SaveChangesAsync();

    }
}