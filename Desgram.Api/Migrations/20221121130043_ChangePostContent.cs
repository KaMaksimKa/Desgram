using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desgram.Api.Migrations
{
    public partial class ChangePostContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostContents_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImagePostCandidates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    ContentImagePostId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImagePostContentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePostCandidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagePostCandidates_Attaches_Id",
                        column: x => x.Id,
                        principalTable: "Attaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImagePostCandidates_PostContents_ImagePostContentId",
                        column: x => x.ImagePostContentId,
                        principalTable: "PostContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagePostCandidates_ImagePostContentId",
                table: "ImagePostCandidates",
                column: "ImagePostContentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContents_PostId",
                table: "PostContents",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagePostCandidates");

            migrationBuilder.DropTable(
                name: "PostContents");
        }
    }
}
