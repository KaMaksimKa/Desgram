using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desgram.Api.Migrations
{
    public partial class ChangeNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Notifications_NotificationId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeComment_Notifications_NotificationId",
                table: "LikeComment");

            migrationBuilder.DropForeignKey(
                name: "FK_LikePost_Notifications_NotificationId",
                table: "LikePost");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscriptions_Notifications_NotificationId",
                table: "UserSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscriptions_NotificationId",
                table: "UserSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_LikePost_NotificationId",
                table: "LikePost");

            migrationBuilder.DropIndex(
                name: "IX_LikeComment_NotificationId",
                table: "LikeComment");

            migrationBuilder.DropIndex(
                name: "IX_Comments_NotificationId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "LikePost");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "LikeComment");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "Comments");

            migrationBuilder.AddColumn<Guid>(
                name: "CommentId",
                table: "Notifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LikeCommentId",
                table: "Notifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LikePostId",
                table: "Notifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                table: "Notifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CommentId",
                table: "Notifications",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_LikeCommentId",
                table: "Notifications",
                column: "LikeCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_LikePostId",
                table: "Notifications",
                column: "LikePostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SubscriptionId",
                table: "Notifications",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Comments_CommentId",
                table: "Notifications",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_LikeComment_LikeCommentId",
                table: "Notifications",
                column: "LikeCommentId",
                principalTable: "LikeComment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_LikePost_LikePostId",
                table: "Notifications",
                column: "LikePostId",
                principalTable: "LikePost",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserSubscriptions_SubscriptionId",
                table: "Notifications",
                column: "SubscriptionId",
                principalTable: "UserSubscriptions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Comments_CommentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_LikeComment_LikeCommentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_LikePost_LikePostId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserSubscriptions_SubscriptionId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CommentId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_LikeCommentId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_LikePostId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SubscriptionId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "LikeCommentId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "LikePostId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Notifications");

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "UserSubscriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "LikePost",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "LikeComment",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "Comments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_NotificationId",
                table: "UserSubscriptions",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_LikePost_NotificationId",
                table: "LikePost",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeComment_NotificationId",
                table: "LikeComment",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NotificationId",
                table: "Comments",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Notifications_NotificationId",
                table: "Comments",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeComment_Notifications_NotificationId",
                table: "LikeComment",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikePost_Notifications_NotificationId",
                table: "LikePost",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscriptions_Notifications_NotificationId",
                table: "UserSubscriptions",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");
        }
    }
}
