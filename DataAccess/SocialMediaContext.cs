﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DataAccess.Entities;

namespace DataAccess
{
    public partial class SocialMediaContext : DbContext
    {
        public SocialMediaContext()
        {
        }

        public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ChannelCategory> ChannelCategories { get; set; }
        public virtual DbSet<ChannelCrawl> ChannelCrawls { get; set; }
        public virtual DbSet<ChannelRecord> ChannelRecords { get; set; }
        public virtual DbSet<Hashtag> Hashtags { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<PostCrawl> PostCrawls { get; set; }
        public virtual DbSet<PostHashtag> PostHashtags { get; set; }
        public virtual DbSet<Reaction> Reactions { get; set; }
        public virtual DbSet<Reactiontype> Reactiontypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<TransactionDeposit> TransactionDeposits { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<Watchlist> Watchlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Brands)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_brand_organization");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_category_platform");
            });

            modelBuilder.Entity<ChannelCategory>(entity =>
            {
                entity.HasKey(e => new { e.ChannelId, e.CategoryId });

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ChannelCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChannelCategory_category");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.ChannelCategories)
                    .HasForeignKey(d => d.ChannelId)
                    .HasConstraintName("FK_ChannelCategory_channel_crawl");
            });

            modelBuilder.Entity<ChannelCrawl>(entity =>
            {
                entity.Property(e => e.Cid).IsUnicode(false);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.ChannelCrawls)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_crawl_location");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.ChannelCrawls)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_crawl_organization");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.ChannelCrawls)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_crawl_platform");
            });

            modelBuilder.Entity<ChannelRecord>(entity =>
            {
                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.ChannelRecords)
                    .HasForeignKey(d => d.ChannelId)
                    .HasConstraintName("FK_channel_record_channel_crawl");
            });


            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);
            });

            modelBuilder.Entity<PostCrawl>(entity =>
            {
                entity.HasKey(e => e.Pid)
                    .HasName("PK_postcrawl");

                entity.Property(e => e.Pid).IsUnicode(false);

                entity.Property(e => e.PostType).IsUnicode(false);

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.PostCrawls)
                    .HasForeignKey(d => d.ChannelId)
                    .HasConstraintName("FK_post_crawl_channel_crawl1");
            });

            modelBuilder.Entity<PostHashtag>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.HashtagId });

                entity.HasOne(d => d.Hashtag)
                    .WithMany(p => p.PostHashtags)
                    .HasForeignKey(d => d.HashtagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_hashtag_hashtag");
            });

            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.HasKey(e => new { e.ReactionTypeId, e.PostId });

                entity.Property(e => e.PostId).IsUnicode(false);

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reaction_post_crawl");

                entity.HasOne(d => d.ReactionType)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.ReactionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reaction_reactiontype");
            });

            modelBuilder.Entity<Reactiontype>(entity =>
            {
                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Reactiontypes)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reactiontype_platform");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Offer)
                    .WithMany()
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_subscription_offer");

                entity.HasOne(d => d.Package)
                    .WithMany()
                    .HasForeignKey(d => d.PackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_subscription_package");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_subscription_user");

                entity.HasOne(d => d.Wallet)
                    .WithMany()
                    .HasForeignKey(d => d.WalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_subscription_wallet");
            });

            modelBuilder.Entity<TransactionDeposit>(entity =>
            {
                entity.Property(e => e.BankCode).IsUnicode(false);

                entity.Property(e => e.CardType).IsUnicode(false);

                entity.Property(e => e.OrderInfor)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.OrderType)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ResponseCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TmnCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.TransactionDeposits)
                    .HasForeignKey(d => d.WalletId)
                    .HasConstraintName("FK_transaction_deposit_wallet");
            });


            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.AccessToken).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FcmToken).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.Provider).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_role");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("FK_user_id_wallet")
                    .IsUnique();

            
                entity.HasOne(d => d.User)
                    .WithOne(p => p.Wallet)
                    .HasForeignKey<Wallet>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_wallet_user");
            });

            modelBuilder.Entity<Watchlist>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ChannelId });

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.Watchlists)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_watchlist_channel_crawl");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Watchlists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_watchlist_user");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
        .Entries()
        .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Modified)
                {
                    ((BaseEntity)entityEntry.Entity).UpdateDate = DateTime.Now;
                    entityEntry.Property("CreatedDate").IsModified = false;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}