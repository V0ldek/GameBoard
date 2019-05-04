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

                    entity.HasIndex(e => new { e.RequestedById, e.RequestedToId })
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
                        .ValueGeneratedOnAdd();

                    entity.Property(e => e.Name)
                        .HasMaxLength(48);

                    entity.Property(e => e.Place)
                        .HasMaxLength(128);

                    entity.Property(e => e.MeetingTime)
                        .HasConversion(new DateTimeToBinaryConverter());

                    entity.ToTable("GameEvent");
                });

            builder.Entity<GameEventParticipation>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd();

                    entity.HasOne(e => e.Paticipant)
                        .WithMany(u => u.Participations)
                        .HasForeignKey(e => e.ParticipantId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(e => e.TakesPartIn)
                        .WithMany(ge => ge.Participations)
                        .HasForeignKey(e => e.TakesPartInId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.Property(e => e.ParticipationStatus)
                        .HasConversion(new EnumToStringConverter<ParticipationStatus>())
                        .IsRequired()
                        .HasDefaultValue(ParticipationStatus.PendingGuest);

                    entity.HasIndex(e => new { e.TakesPartInId, e.ParticipantId })
                        .HasFilter("ParticipationStatus <> 'RejectedGuest'")
                        .IsUnique();

                    entity.HasIndex(e => e.ParticipantId);

                    entity.HasIndex(e => e.TakesPartInId)
                        .HasFilter("ParticipationStatus = 'Creator'")
                        .IsUnique();

                    entity.ToTable("GameEventParticipation");
                });

            builder.Entity<Game>(
                entity =>
                {
                    //Perhaps we could have an auto generated key and an enum which tells us whether the game is on the current list or it was deleted from it.
                    entity.Property(e => e.Name)
                        .IsRequired()
                        .HasMaxLength(128);

                    entity.HasOne(e => e.GameEvent)
                        .WithMany(g => g.Games)
                        .HasForeignKey(e => e.GameEventId)
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasKey(e => new { e.GameEventId, e.Name });

                    entity.ToTable("Game");
                });
        }
    }
}