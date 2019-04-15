using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Repositories
{
    public interface IGameBoardRepository
    {
        DbSet<ApplicationUser> ApplicationUsers { get; }
        DbSet<Friendship> Friendships { get; }

        Task<string> GetUserIdByUsername(string userName);

        Task SaveChangesAsync();
    }
}