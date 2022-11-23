using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desgram.Api.Migrations
{
    public partial class ChangeImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_Attaches_Id",
                table: "Avatars");

            migrationBuilder.DropTable(
                name: "AttachesPosts");

            migrationBuilder.DropTable(
                name: "ImagePostCandidates");

            migrationBuilder.DropTable(
                name: "PostContents");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Attaches");

            migrationBuilder.CreateTable(
                name: "ImageContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    ImageContentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Attaches_Id",
                        column: x => x.Id,
                        principalTable: "Attaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_ImageContents_ImageContentId",
                        column: x => x.ImageContentId,
                        principalTable: "ImageContents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostImageContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImageContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostImageContents_ImageContents_Id",
                        column: x => x.Id,
                        principalTable: "ImageContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostImageContents_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageContentId",
                table: "Images",
                column: "ImageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostImageContents_PostId",
                table: "PostImageContents",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_ImageContents_Id",
                table: "Avatars",
                column: "Id",
                principalTable: "ImageContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatars_ImageContents_Id",
                table: "Avatars");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "PostImageContents");

            migrationBuilder.DropTable(
                name: "ImageContents");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Attaches",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "AttachesPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachesPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachesPosts_Attaches_Id",
                        column: x => x.Id,
                        principalTable: "Attaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachesPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    ImagePostContentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentImagePostId = table.Column<Guid>(type: "uuid", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_AttachesPosts_PostId",
                table: "AttachesPosts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePostCandidates_ImagePostContentId",
                table: "ImagePostCandidates",
                column: "ImagePostContentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContents_PostId",
                table: "PostContents",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avatars_Attaches_Id",
                table: "Avatars",
                column: "Id",
                principalTable: "Attaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
