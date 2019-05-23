using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Transactions;
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
        DbSet<Group> Groups { get; }
        DbSet<GroupUser> GroupUsers { get; }
        DbSet<DescriptionTab> DescriptionTabs { get; }

        ITransaction BeginTransaction();

        Task SaveChangesAsync();
    }
}