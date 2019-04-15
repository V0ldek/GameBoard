﻿using GameBoard.DataLayer.Entities;
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
        }

        public Task SaveChangesAsync() => base.SaveChangesAsync();
    }
}