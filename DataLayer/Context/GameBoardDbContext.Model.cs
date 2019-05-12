using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameBoard.DataLayer.Context
{
    internal sealed partial class GameBoardDbContext
    {
        public Task SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(
                entity =>
                {
                    entity.Property(e => e.UserName).HasMaxLength(16);
                    entity.Property(e => e.NormalizedUserName).HasMaxLength(16);

                    entity.HasMany(u => u.GroupUser)
                        .WithOne(gu => gu.User)
                        .HasForeignKey(gu => gu.UserId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.ToTable("User");
                });

            builder.Entity<Friendship>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd();

                    entity.Property(e => e.FriendshipStatus)
                        .IsRequired();

                    entity.HasIndex(e => new {e.RequestedById, e.RequestedToId})
                        .HasFilter("FriendshipStatus <> 1") // <> rejected
                        .IsUnique();

                    entity.HasOne(e => e.RequestedBy)
                        .WithMany(u => u.SentRequests)
                        .HasForeignKey(e => e.RequestedById)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(e => e.RequestedTo)
                        .WithMany(u => u.ReceivedRequests)
                        .HasForeignKey(e => e.RequestedToId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.ToTable("Friendship");
                });

            builder.Entity<Group>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd();

                    entity.HasOne(e => e.Owner)
                        .WithMany(u => u.UserGroups)
                        .HasForeignKey(e => e.OwnerId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasMany(g => g.GroupUser)
                        .WithOne(gu => gu.Group)
                        .HasForeignKey(gu => gu.GroupId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.Ignore(g => g.Users);

                    entity.Property(e => e.Name).HasMaxLength(64);

                    entity.ToTable("Group");
                });

            builder.Entity<GroupUser>().HasKey(
                entity => new {entity.GroupId, entity.UserId });

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
    }
}