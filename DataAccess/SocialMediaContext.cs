using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<ChannelCrawl> ChannelCrawls { get; set; }
        public virtual DbSet<ChannelDetail> ChannelDetails { get; set; }
        public virtual DbSet<ChannelManagement> ChannelManagements { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Criterion> Criteria { get; set; }
        public virtual DbSet<Gateway> Gateways { get; set; }
        public virtual DbSet<Hashtag> Hashtags { get; set; }
        public virtual DbSet<HashtagGroup> HashtagGroups { get; set; }
        public virtual DbSet<HashtagPost> HashtagPosts { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MediaItem> MediaItems { get; set; }
        public virtual DbSet<MediaType> MediaTypes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostCrawl> PostCrawls { get; set; }
        public virtual DbSet<PostDetail> PostDetails { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<RankType> RankTypes { get; set; }
        public virtual DbSet<Reaction> Reactions { get; set; }
        public virtual DbSet<ReactionType> ReactionTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Target> Targets { get; set; }
        public virtual DbSet<TargetType> TargetTypes { get; set; }
        public virtual DbSet<TransactionDeposit> TransactionDeposits { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }

   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.PlatformId).HasColumnName("platform_id");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.PlatformId)
                    .HasConstraintName("FK_category_platform");
            });

            modelBuilder.Entity<Channel>(entity =>
            {
                entity.ToTable("channel");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("avatar");

                entity.Property(e => e.Banner)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("banner");

                entity.Property(e => e.Cid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cid");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.CriteriaId).HasColumnName("criteria_id");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nickname");

                entity.Property(e => e.OrganizationName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("organization_name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.PlatformId).HasColumnName("platform_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("url");

                entity.HasOne(d => d.Criteria)
                    .WithMany(p => p.Channels)
                    .HasForeignKey(d => d.CriteriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_criteria");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Channel)
                    .HasForeignKey<Channel>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_channel_crawl");

                entity.HasOne(d => d.Id1)
                    .WithOne(p => p.Channel)
                    .HasForeignKey<Channel>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_ChannelManagement");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Channels)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_location");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Channels)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_member");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Channels)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_platform");
            });

            modelBuilder.Entity<ChannelCrawl>(entity =>
            {
                entity.ToTable("channel_crawl");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
            });

            modelBuilder.Entity<ChannelDetail>(entity =>
            {
                entity.ToTable("channel_detail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChannelId).HasColumnName("channel_id");

                entity.Property(e => e.TotalComment).HasColumnName("total_comment");

                entity.Property(e => e.TotalFollow).HasColumnName("total_follow");

                entity.Property(e => e.TotalLike).HasColumnName("total_like");

                entity.Property(e => e.TotalPost).HasColumnName("total_post");

                entity.Property(e => e.TotalShare).HasColumnName("total_share");

                entity.Property(e => e.TotalView).HasColumnName("total_view");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.ChannelDetails)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_channel_detail_channel");
            });

            modelBuilder.Entity<ChannelManagement>(entity =>
            {
                entity.ToTable("ChannelManagement");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.TargetId).HasColumnName("target_id");

                entity.Property(e => e.TotalComment).HasColumnName("total_comment");

                entity.Property(e => e.TotalFollow).HasColumnName("total_follow");

                entity.Property(e => e.TotalLike).HasColumnName("total_like");

                entity.Property(e => e.TotalPost).HasColumnName("total_post");

                entity.Property(e => e.TotalShare).HasColumnName("total_share");

                entity.Property(e => e.TotalView).HasColumnName("total_view");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.ChannelManagements)
                    .HasForeignKey(d => d.TargetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChannelManagement_target");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthorId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("author_id");

                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("author_name");

                entity.Property(e => e.AuthorUsername)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("author_username");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.ReplyId).HasColumnName("reply_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_post_crawl");
            });

            modelBuilder.Entity<Criterion>(entity =>
            {
                entity.ToTable("criteria");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Gateway>(entity =>
            {
                entity.ToTable("gateway");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccessKey)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("access_key");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.BankCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bank_code");

                entity.Property(e => e.BankTransNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bank_trans_no");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.SecretKey)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("secret_key");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("type")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Gateways)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GateWay_admin");
            });

            modelBuilder.Entity<Hashtag>(entity =>
            {
                entity.ToTable("hashtag");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<HashtagGroup>(entity =>
            {
                entity.ToTable("hashtag_group");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.HashtagId).HasColumnName("hashtag_id");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.HashtagGroups)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_hashtag_group_member");
            });

            modelBuilder.Entity<HashtagPost>(entity =>
            {
                entity.ToTable("hashtag_post");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.HashtagPosts)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_hashtag_post_hashtag_group");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<MediaItem>(entity =>
            {
                entity.ToTable("media_item");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.MediaTypeId).HasColumnName("media_type_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Size).HasColumnName("size");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("url");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.HasOne(d => d.MediaType)
                    .WithMany(p => p.MediaItems)
                    .HasForeignKey(d => d.MediaTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_media_item_media_type");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.MediaItems)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_media_item_posts");
            });

            modelBuilder.Entity<MediaType>(entity =>
            {
                entity.ToTable("media_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.FcmToken)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("fcm_token");

                entity.Property(e => e.WalletId).HasColumnName("wallet_id");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.WalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_member_wallet");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChannelId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("channel_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("note");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TotalMoney).HasColumnName("total_money");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_member");
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("platform");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasColumnName("body");

                entity.Property(e => e.ChannelId).HasColumnName("channel_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.HashtagId).HasColumnName("hashtag_id");

                entity.Property(e => e.Pid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pid");

                entity.Property(e => e.PostTypeId).HasColumnName("post_type_id");

                entity.Property(e => e.ScheduleTime)
                    .HasColumnType("datetime")
                    .HasColumnName("schedule_time");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_posts_ChannelManagement");

                entity.HasOne(d => d.Hashtag)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.HashtagId)
                    .HasConstraintName("FK_posts_hashtag");
            });

            modelBuilder.Entity<PostCrawl>(entity =>
            {
                entity.ToTable("post_crawl");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("body");

                entity.Property(e => e.ChannelId).HasColumnName("channel_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.HashtagId).HasColumnName("hashtag_id");

                entity.Property(e => e.Pid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pid");

                entity.Property(e => e.PostType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("post_type");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.UpdateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("update_at");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.PostCrawls)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_crawl_channel_crawl");

                entity.HasOne(d => d.Hashtag)
                    .WithMany(p => p.PostCrawls)
                    .HasForeignKey(d => d.HashtagId)
                    .HasConstraintName("FK_post_crawl_hashtag");
            });

            modelBuilder.Entity<PostDetail>(entity =>
            {
                entity.ToTable("post_detail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TotalComment).HasColumnName("total_comment");

                entity.Property(e => e.TotalLike).HasColumnName("total_like");

                entity.Property(e => e.TotalShare).HasColumnName("total_share");

                entity.Property(e => e.TotalView).HasColumnName("total_view");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostDetails)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_detail_post_crawl");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.ToTable("rank");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChannelId).HasColumnName("channel_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Grade).HasColumnName("grade");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.RankTypeId).HasColumnName("rank_type_id");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Channel)
                    .WithMany(p => p.Ranks)
                    .HasForeignKey(d => d.ChannelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_rank_channel");

                entity.HasOne(d => d.RankType)
                    .WithMany(p => p.Ranks)
                    .HasForeignKey(d => d.RankTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_rank_rank_type");
            });

            modelBuilder.Entity<RankType>(entity =>
            {
                entity.ToTable("rank_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.ToTable("reaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("display_name");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.ReactionTypeId).HasColumnName("reaction_type_id");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reaction_post_crawl");

                entity.HasOne(d => d.ReactionType)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(d => d.ReactionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reaction_reaction_type");
            });

            modelBuilder.Entity<ReactionType>(entity =>
            {
                entity.ToTable("reaction_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Target>(entity =>
            {
                entity.ToTable("target");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChannelId).HasColumnName("channel_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_date");

                entity.Property(e => e.Progress).HasColumnName("progress");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TargetTypeId).HasColumnName("target_type_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.TargetType)
                    .WithMany(p => p.Targets)
                    .HasForeignKey(d => d.TargetTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_target_target_type");
            });

            modelBuilder.Entity<TargetType>(entity =>
            {
                entity.ToTable("target_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Field)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("field");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TransactionDeposit>(entity =>
            {
                entity.ToTable("transaction_deposit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.CardType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("card_type")
                    .IsFixedLength(true);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("code")
                    .IsFixedLength(true);

                entity.Property(e => e.CurrentBlance).HasColumnName("current_blance");

                entity.Property(e => e.GatewayId).HasColumnName("gateway_id");

                entity.Property(e => e.Locale)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.NewBalance).HasColumnName("new_balance");

                entity.Property(e => e.OrderInfor)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("order_infor")
                    .IsFixedLength(true);

                entity.Property(e => e.OrderType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("order_type")
                    .IsFixedLength(true);

                entity.Property(e => e.PayDate)
                    .HasColumnType("datetime")
                    .HasColumnName("pay_date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TmnCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("tmn_code")
                    .IsFixedLength(true);

                entity.Property(e => e.TransNoId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("trans_no_id")
                    .IsFixedLength(true);

                entity.Property(e => e.TxnRef)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("txn_ref")
                    .IsFixedLength(true);

                entity.Property(e => e.WalletId).HasColumnName("wallet_id");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.TransactionDeposits)
                    .HasForeignKey(d => d.WalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_transaction_deposit_wallet");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.LastLoginAt)
                    .HasColumnType("datetime")
                    .HasColumnName("last_login_at");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_admin");

                entity.HasOne(d => d.Id1)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_member");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_role");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("wallet");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("currency")
                    .IsFixedLength(true);

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("status")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_wallet_member");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
