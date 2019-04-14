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
                entity =>
                {
                    entity.ToTable("user");
                });

            builder.Entity<IdentityRole>(
                entity => entity.ToTable("role"));


            builder.Entity<IdentityUserRole<string>>(
                entity => entity.ToTable("user_role"));

            builder.Entity<IdentityUserClaim<string>>(
                entity => entity.ToTable("user_claim"));

            builder.Entity<IdentityUserLogin<string>>(
                entity => entity.ToTable("user_login"));

            builder.Entity<IdentityUserToken<string>>(
                entity => entity.ToTable("user_token"));

            builder.Entity<IdentityRoleClaim<string>>(
                entity => entity.ToTable("role_claim"));
        }
    }
}