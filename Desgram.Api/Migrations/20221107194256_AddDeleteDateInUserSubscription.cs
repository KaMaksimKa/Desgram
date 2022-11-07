using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desgram.Api.Migrations
{
    public partial class AddDeleteDateInUserSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "UserSubscriptions",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "UserSubscriptions");
        }
    }
}
