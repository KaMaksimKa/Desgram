using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desgram.Api.Migrations
{
    public partial class AddHashTags3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HashTagPublication_HashTags_TagsId",
                table: "HashTagPublication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HashTagPublication",
                table: "HashTagPublication");

            migrationBuilder.DropIndex(
                name: "IX_HashTagPublication_TagsId",
                table: "HashTagPublication");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "HashTagPublication",
                newName: "HashTagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HashTagPublication",
                table: "HashTagPublication",
                columns: new[] { "HashTagsId", "PublicationsId" });

            migrationBuilder.CreateIndex(
                name: "IX_HashTagPublication_PublicationsId",
                table: "HashTagPublication",
                column: "PublicationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_HashTagPublication_HashTags_HashTagsId",
                table: "HashTagPublication",
                column: "HashTagsId",
                principalTable: "HashTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HashTagPublication_HashTags_HashTagsId",
                table: "HashTagPublication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HashTagPublication",
                table: "HashTagPublication");

            migrationBuilder.DropIndex(
                name: "IX_HashTagPublication_PublicationsId",
                table: "HashTagPublication");

            migrationBuilder.RenameColumn(
                name: "HashTagsId",
                table: "HashTagPublication",
                newName: "TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HashTagPublication",
                table: "HashTagPublication",
                columns: new[] { "PublicationsId", "TagsId" });

            migrationBuilder.CreateIndex(
                name: "IX_HashTagPublication_TagsId",
                table: "HashTagPublication",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_HashTagPublication_HashTags_TagsId",
                table: "HashTagPublication",
                column: "TagsId",
                principalTable: "HashTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
