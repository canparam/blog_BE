using Microsoft.EntityFrameworkCore.Migrations;

namespace can.blog.Data.Migrations
{
    public partial class addedtablev4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Categories",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "OgImage",
                table: "Categories",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_TagPosts_TagId",
                table: "TagPosts",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagPosts_Tags_TagId",
                table: "TagPosts",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagPosts_Tags_TagId",
                table: "TagPosts");

            migrationBuilder.DropIndex(
                name: "IX_TagPosts_TagId",
                table: "TagPosts");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Categories",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OgImage",
                table: "Categories",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
