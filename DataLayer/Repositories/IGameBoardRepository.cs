using System.Linq;
using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Repositories
{
    public interface IGameBoardRepository
    {

        DbSet<ApplicationUser> ApplicationUsers { get; }
        DbSet<Friendship> Friendships { get; }
        DbSet<GameEvent> GameEvents { get; }
        DbSet<GameEventParticipation> GameEventParticipations { get; }
        DbSet<Game> Games { get; }

        IQueryable<ApplicationUser> GetUserByUserName(string userName);
        Task<string> GetUserIdByUserName(string userName);

        Task SaveChangesAsync();
    }
}