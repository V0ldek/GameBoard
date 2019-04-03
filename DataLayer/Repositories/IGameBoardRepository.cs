using GameBoard.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Repositories
{
    public interface IGameBoardRepository
    {
        DbSet<ApplicationUser> ApplicationUsers { get; set; } //only get?
        DbSet<FriendRequest> FriendRequests { get; set; } //only set?
    }
}