using GameBoard.DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GameBoard.DataLayer.Context
{
    internal sealed partial class GameBoardDbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(
                entity =>
                {
                    entity.HasIndex(e => e.UserName);
                    entity.ToTable("user");
                });

            builder.Entity<Friendship>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Id)
                        .HasMaxLength(32)
                        .IsRequired();
                    entity.Property(e => e.FriendshipStatus)
                        .IsRequired();

                    entity.HasIndex(e => new {RequestedById = e.RequestedById, RequestedToId = e.RequestedToId})
                        .IsUnique(true); //is it directed uniqueness?

                    entity.HasOne(e => e.RequestedBy)
                        .WithMany(u => u.SentRequests)
                        .HasForeignKey(e => e.RequestedById);

                    entity.HasOne(e => e.RequestedTo)
                        .WithMany(u => u.ReceivedRequests)
                        .HasForeignKey(e => e.RequestedToId);
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

        public Task SaveChangesAsync() => base.SaveChangesAsync();
    }
}