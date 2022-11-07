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

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Attaches");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AmountLikes")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PublicationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PublicationId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
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

                    b.ToTable("HashTags");
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

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Publication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AmountComments")
                        .HasColumnType("integer");

                    b.Property<int>("AmountLikes")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Publications");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AmountSubscribers")
                        .HasColumnType("integer");

                    b.Property<int>("AmountSubscriptions")
                        .HasColumnType("integer");

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

                    b.ToTable("Users");
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

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.UserSubscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SubscriberId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubscriptionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SubscriberId");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("UserSubscriptions");
                });

            modelBuilder.Entity("HashTagPublication", b =>
                {
                    b.Property<Guid>("HashTagsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PublicationsId")
                        .HasColumnType("uuid");

                    b.HasKey("HashTagsId", "PublicationsId");

                    b.HasIndex("PublicationsId");

                    b.ToTable("HashTagPublication");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.AttachPublication", b =>
                {
                    b.HasBaseType("Desgram.DAL.Entities.Attach");

                    b.Property<Guid>("PublicationId")
                        .HasColumnType("uuid");

                    b.HasIndex("PublicationId");

                    b.ToTable("AttachPublications", (string)null);
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Avatar", b =>
                {
                    b.HasBaseType("Desgram.DAL.Entities.Attach");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasIndex("UserId")
                        .IsUnique();

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

            modelBuilder.Entity("Desgram.DAL.Entities.LikePublication", b =>
                {
                    b.HasBaseType("Desgram.DAL.Entities.Like");

                    b.Property<Guid>("PublicationId")
                        .HasColumnType("uuid");

                    b.HasIndex("PublicationId");

                    b.ToTable("LikePublication", (string)null);
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

            modelBuilder.Entity("Desgram.DAL.Entities.Comment", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Publication", "Publication")
                        .WithMany("Comments")
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publication");

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

            modelBuilder.Entity("Desgram.DAL.Entities.Publication", b =>
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
                    b.HasOne("Desgram.DAL.Entities.User", "Subscriber")
                        .WithMany("Subscriptions")
                        .HasForeignKey("SubscriberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.User", "Subscription")
                        .WithMany("Subscribers")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscriber");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("HashTagPublication", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.HashTag", null)
                        .WithMany()
                        .HasForeignKey("HashTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.Publication", null)
                        .WithMany()
                        .HasForeignKey("PublicationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Desgram.DAL.Entities.AttachPublication", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Attach", null)
                        .WithOne()
                        .HasForeignKey("Desgram.DAL.Entities.AttachPublication", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.Publication", "Publication")
                        .WithMany("AttachPublications")
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publication");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Avatar", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Attach", null)
                        .WithOne()
                        .HasForeignKey("Desgram.DAL.Entities.Avatar", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.User", "User")
                        .WithOne("Avatar")
                        .HasForeignKey("Desgram.DAL.Entities.Avatar", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.LikeComment", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Comment", "Comment")
                        .WithMany("LikesComment")
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

            modelBuilder.Entity("Desgram.DAL.Entities.LikePublication", b =>
                {
                    b.HasOne("Desgram.DAL.Entities.Like", null)
                        .WithOne()
                        .HasForeignKey("Desgram.DAL.Entities.LikePublication", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desgram.DAL.Entities.Publication", "Publication")
                        .WithMany("LikesPublication")
                        .HasForeignKey("PublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publication");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Comment", b =>
                {
                    b.Navigation("LikesComment");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.Publication", b =>
                {
                    b.Navigation("AttachPublications");

                    b.Navigation("Comments");

                    b.Navigation("LikesPublication");
                });

            modelBuilder.Entity("Desgram.DAL.Entities.User", b =>
                {
                    b.Navigation("Avatar");

                    b.Navigation("Sessions");

                    b.Navigation("Subscribers");

                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
