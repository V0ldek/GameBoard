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
                entity => entity.ToTable("User"));

            builder.Entity<Friendship>(
                entity =>
                {
                    entity.HasKey(e => e.Id);

                    entity.Property(e => e.Id)
                        .HasMaxLength(32)
                        .IsRequired();
                    entity.Property(e => e.FriendshipStatus)
                        .IsRequired();

                    entity.Property(e => e.RequestedById)
                        .IsRequired();
                    entity.Property(e => e.RequestedToId)
                        .IsRequired();

                    entity.HasIndex(e => new {RequestedById = e.RequestedById, RequestedToId = e.RequestedToId})
                        .IsUnique(true); //is it directed uniqueness?

                    entity.HasOne(e => e.RequestedBy)
                        .WithMany(u => u.SentRequests)
                        .HasForeignKey(e => e.RequestedById)
                        //.OnDelete(DeleteBehavior.Restrict);
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(e => e.RequestedTo)
                        .WithMany(u => u.ReceivedRequests)
                        .HasForeignKey(e => e.RequestedToId)
                        //.OnDelete(DeleteBehavior.Restrict);
                        .OnDelete(DeleteBehavior.Restrict);
                });

            builder.Entity<IdentityRole>(
                entity => entity.ToTable("Role"));

            builder.Entity<IdentityUserRole<string>>(
                entity => entity.ToTable("UserRole"));

            builder.Entity<IdentityUserClaim<string>>(
                entity => entity.ToTable("UserClaim"));

            builder.Entity<IdentityUserLogin<string>>(
                entity => entity.ToTable("UserLogin"));

            builder.Entity<IdentityUserToken<string>>(
                entity => entity.ToTable("UserToken"));

            builder.Entity<IdentityRoleClaim<string>>(
                entity => entity.ToTable("RoleClaim"));
        }

        public Task SaveChangesAsync() => base.SaveChangesAsync();
    }
}