using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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

        IDbContextTransaction BeginTransaction();

        Task SaveChangesAsync();
    }
}