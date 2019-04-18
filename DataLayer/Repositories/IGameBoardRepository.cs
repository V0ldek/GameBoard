using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Repositories
{
    public interface IGameBoardRepository
    {

        DbSet<ApplicationUser> ApplicationUsers { get; }
        DbSet<Friendship> Friendships { get; }
        DbSet<GameEvent> GameEvents { get; set; }

        Task<string> GetUserIdByUserName(string userName);

        Task SaveChangesAsync();
    }
}