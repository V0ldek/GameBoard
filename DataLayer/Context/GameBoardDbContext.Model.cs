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
                entity => entity.ToTable("user"));

            builder.Entity<Friendship>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Id)
                        .HasMaxLength(32)
                        .IsRequired();
                    entity.Property(e => e.FriendshipStatus)
                        .IsRequired();

                    entity.HasIndex(e => new {UserSmallerId = e.UserSmallerId, UserGreaterId = e.UserGreaterId})
                        // CKECH(UserLessId <= UserGreaterId)
                        .IsUnique(true); //is it directed uniqueness?

                    entity.HasOne(e => e.UserSmaller)
                        .WithMany(u => u.SmallerIdInFriendships)
                        .HasForeignKey(e => e.UserSmallerId);

                    entity.HasOne(e => e.UserGreater)
                        .WithMany(u => u.GreaterIdInFriendships)
                        .HasForeignKey(e => e.UserGreaterId);
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