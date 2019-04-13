using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Context
{
    internal sealed partial class GameBoardDbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>(
                entity => entity.ToTable("User"));

            builder.Entity<IdentityRole>( 
                entity => entity.ToTable("Role"));

            builder.Entity<IdentityUserRole<string>>(
                entity => entity.ToTable("UserRole"));

            builder.Entity<IdentityUserClaim<string>>(
                entity => entity.ToTable("UserClaim"));

            builder.Entity<IdentityUserLogin<string>>(
                entity => entity.ToTable("UserLogin"));

            builder.Entity<IdentityUserToken<string>>(
                entity =>entity.ToTable("UserToken"));

            builder.Entity<IdentityRoleClaim<string>>(
                entity => entity.ToTable("RoleClaim"));
        }
    }
}