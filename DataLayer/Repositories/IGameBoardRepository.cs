using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Repositories
{
    public interface IGameBoardRepository
    {
        DbSet<ApplicationUser> ApplicationUsers { get; set; } //only get?
        DbSet<Friendship> Friendships { get; set; } //only get?

        Task SaveChangesAsync();
    }
}