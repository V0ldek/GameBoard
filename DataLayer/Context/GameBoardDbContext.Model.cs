using GameBoard.DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Context
{
    internal sealed partial class GameBoardDbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(
                entity => entity.ToTable("user"));

            builder.Entity<FriendRequest>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Id)
                        .HasMaxLength(32)
                        .IsRequired();
                    entity.Property(e => e.FriendRequestStatus)
                        .IsRequired();

                    entity.HasOne(e => e.UserFrom)
                        .WithMany(u => u.SentRequests)
                        .HasForeignKey(e => e.UserFromId);

                    entity.HasOne(e => e.UserTo)
                        .WithMany(u => u.ReceivedRequests)
                        .HasForeignKey(e => e.UserToId);
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