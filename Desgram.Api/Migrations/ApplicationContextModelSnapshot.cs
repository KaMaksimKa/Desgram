﻿// <auto-generated />
using System;
using Desgram.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Desgram.Api.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Desgram.DAL.Entities.Attach", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Attaches", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.BlockingUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BlockedId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BlockedId");

                    b.HasIndex("UserId");

                    b.ToTable("BlockingUsers", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.HashTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("HashTags", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Likes", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsCommentsEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsLikesVisible")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.SubscriptionRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContentMakerId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ContentMakerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("SubscriptionRequests", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.UnconfirmedEmail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CodeHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("ExpiredDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UnconfirmedEmails", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.UnconfirmedUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CodeHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("ExpiredDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UnconfirmedUsers", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Biography")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.UserSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RefreshTokenId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSessions", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.UserSubscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContentMakerId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ContentMakerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("UserSubscriptions", (string)null);
                });

            modelBuilder.Entity("HashTagPost", b =>
                {
                    b.Property<Guid>("HashTagsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PostsId")
                        .HasColumnType("uuid");

                    b.HasKey("HashTagsId", "PostsId");

                    b.HasIndex("PostsId");

                    b.ToTable("HashTagPost", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.AttachPost", b =>
                {
                    b.HasBaseType("Desgram.DAL.Entities.Attach");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.HasIndex("PostId");

                    b.ToTable("AttachesPosts", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Avatar", b =>
                {
                    b.HasBaseType("Desgram.DAL.Entities.Attach");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasIndex("UserId");

                    b.ToTable("Avatars", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.LikeComment", b =>
                {
                    b.HasBaseType("Desgram.DAL.Entities.Like");

                    b.Property<Guid>("CommentId")
                        .HasColumnType("uuid");

                    b.HasIndex("CommentId");

                    b.ToTable("LikeComment", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.LikePost", b =>
                {
                    b.HasBaseType("Desgram.DAL.Entities.Like");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.HasIndex("PostId");

                    b.ToTable("LikePost", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Attach", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.BlockingUser", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.User", "Blocked")
                        .WithMany()
                        .HasForeignKey("BlockedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.User", "User")
                        .WithMany("BlockedUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blocked");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Comment", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Like", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Post", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.SubscriptionRequest", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.User", "ContentMaker")
                        .WithMany("SubscriptionRequests")
                        .HasForeignKey("ContentMakerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.User", "Follower")
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContentMaker");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.UnconfirmedEmail", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.UserSession", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.UserSubscription", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.User", "ContentMaker")
                        .WithMany("Followers")
                        .HasForeignKey("ContentMakerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.User", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContentMaker");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("HashTagPost", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.HashTag", null)
                        .WithMany()
                        .HasForeignKey("HashTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.Post", null)
                        .WithMany()
                        .HasForeignKey("PostsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Desgram.DAL.Entities.AttachPost", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Attach", null)
                        .WithOne()
                        .HasForeignKey("Desgram.DAL.Entities.AttachPost", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.Post", "Post")
                        .WithMany("Attaches")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Avatar", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Attach", null)
                        .WithOne()
                        .HasForeignKey("Desgram.DAL.Entities.Avatar", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.User", "User")
                        .WithMany("Avatars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.LikeComment", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Comment", "Comment")
                        .WithMany("Likes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.Like", null)
                        .WithOne()
                        .HasForeignKey("Desgram.DAL.Entities.LikeComment", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.LikePost", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Like", null)
                        .WithOne()
                        .HasForeignKey("Desgram.DAL.Entities.LikePost", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Comment", b =>
                {
                    b.Navigation("Likes");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Post", b =>
                {
                    b.Navigation("Attaches");

                    b.Navigation("Comments");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.User", b =>
                {
                    b.Navigation("Avatars");

                    b.Navigation("BlockedUsers");

                    b.Navigation("Followers");

                    b.Navigation("Following");

                    b.Navigation("Posts");

                    b.Navigation("Sessions");

                    b.Navigation("SubscriptionRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
