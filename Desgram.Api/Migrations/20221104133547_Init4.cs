using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desgram.Api.Migrations
{
    public partial class Init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_Images_Id",
                table: "Avatars");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_OwnerId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagesPublications_Images_Id",
                table: "ImagesPublications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Attaches");

            migrationBuilder.RenameIndex(
                name: "IX_Images_OwnerId",
                table: "Attaches",
                newName: "IX_Attaches_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attaches",
                table: "Attaches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attaches_Users_OwnerId",
                table: "Attaches",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Attaches_Id",
                table: "Avatars",
                column: "Id",
                principalTable: "Attaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImagesPublications_Attaches_Id",
                table: "ImagesPublications",
                column: "Id",
                principalTable: "Attaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attaches_Users_OwnerId",
                table: "Attaches");

            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_Attaches_Id",
                table: "Avatars");

            migrationBuilder.DropForeignKey(
                name: "FK_ImagesPublications_Attaches_Id",
                table: "ImagesPublications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attaches",
                table: "Attaches");

            migrationBuilder.RenameTable(
                name: "Attaches",
                newName: "Images");

            migrationBuilder.RenameIndex(
                name: "IX_Attaches_OwnerId",
                table: "Images",
                newName: "IX_Images_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Images_Id",
                table: "Avatars",
                column: "Id",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_OwnerId",
                table: "Images",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImagesPublications_Images_Id",
                table: "ImagesPublications",
                column: "Id",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
