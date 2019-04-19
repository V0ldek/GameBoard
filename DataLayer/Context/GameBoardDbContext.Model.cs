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
                    
                    entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(512);

                    entity.HasOne(e => e.Creator)
                        .WithMany(c => c.CreatedEvents)
                        .HasForeignKey(e => e.CreatorId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.Property(e => e.MeetingTime)
                        .HasConversion(new DateTimeToBinaryConverter());
                    
                    entity.ToTable("GameEvent");
                });

            builder.Entity<GameEventInvitation>(
                entity =>
                {
                    entity.HasOne(e => e.SendTo)
                        .WithMany(u => u.Invitations)
                        .HasForeignKey(e => e.SendToId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(e => e.InvitedTo)
                        .WithMany(ge => ge.Invitations)
                        .HasForeignKey(e => e.InvitedToId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.Property(e => e.InvitationStatus)
                        .HasConversion(new EnumToStringConverter<InvitationStatus>())
                        .IsRequired()
                        .HasDefaultValue(InvitationStatus.Pending);

                    entity.HasKey(e => new {e.SendToId, e.InvitedToId});

                    entity.HasIndex(e => new {e.InvitedToId});

                    entity.ToTable("GameEventInvitation");
                });

            builder.Entity<Game>(
                entity =>
                {
                    entity.Property(e => e.Name)
                        .IsRequired()
                        .HasMaxLength(256);

                    entity.HasOne(e => e.GameEvent)
                        .WithMany( g => g.Games)
                        .HasForeignKey(e => e.GameEventId)
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasKey(e => new {e.GameEventId, e.Name});

                    entity.ToTable("Game");
                });
            }
    }
}