using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desgram.Api.Migrations
{
    public partial class AddUsersBlockedMeToUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "UserSubscriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_NotificationId",
                table: "UserSubscriptions",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscriptions_Notifications_NotificationId",
                table: "UserSubscriptions",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscriptions_Notifications_NotificationId",
                table: "UserSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscriptions_NotificationId",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "UserSubscriptions");
        }
    }
}
