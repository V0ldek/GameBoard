using System.Threading.Tasks;
using GameBoard.DataLayer.Entities;
using GameBoard.DataLayer.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

            builder.Entity<GameEvent>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id).ValueGeneratedOnAdd();

                    entity.HasOne(e => e.Creator)
                        .WithMany()
                        .HasForeignKey(e => e.CreatorId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.Property(e => e.MeetingTime)
                        .HasConversion(new DateTimeToBinaryConverter());

                    entity.HasMany(e => e.Invitations)
                        .WithOne();

                    entity.HasMany(e => e.Games)
                        .WithOne();

                    entity.ToTable("GameEvent");
                });

            builder.Entity<GameEventInvitation>(
                entity =>
                {
                    entity.HasOne(e => e.SendTo)
                        .WithMany()
                        .HasForeignKey(e => e.SendToId)
                        .IsRequired();

                    entity.HasOne(e => e.InvitedTo)
                        .WithMany()
                        .HasForeignKey(e => e.InvitedToId);

                    entity.Property(e => e.InvitationStatus)
                        .HasConversion(new EnumToStringConverter<InvitationStatus>())
                        .IsRequired()
                        .HasDefaultValue(InvitationStatus.Pending);

                    entity.HasKey(e => new {e.SendTo, e.InvitedTo});

                    entity.HasIndex(e => new {e.InvitedTo});

                    entity.ToTable("GameEventInvitation");
                });

            builder.Entity<Game>(
                entity =>
                {
                    entity.Property(e => e.Name)
                        .IsRequired();

                    entity.HasOne<GameEvent>()
                        .WithMany()
                        .HasForeignKey(e => e.GameEventId);

                    entity.HasKey(e => new {e.GameEventId, e.Name});

                    entity.ToTable("Game");
                });
            }
    }
}