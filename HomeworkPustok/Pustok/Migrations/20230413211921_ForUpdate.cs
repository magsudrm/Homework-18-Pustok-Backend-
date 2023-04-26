using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pustok.Migrations
{
    public partial class ForUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookImage_Books_BookId",
                table: "BookImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookImage",
                table: "BookImage");

            migrationBuilder.RenameTable(
                name: "BookImage",
                newName: "BookImages");

            migrationBuilder.RenameIndex(
                name: "IX_BookImage_BookId",
                table: "BookImages",
                newName: "IX_BookImages_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookImages",
                table: "BookImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookImages_Books_BookId",
                table: "BookImages",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookImages_Books_BookId",
                table: "BookImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookImages",
                table: "BookImages");

            migrationBuilder.RenameTable(
                name: "BookImages",
                newName: "BookImage");

            migrationBuilder.RenameIndex(
                name: "IX_BookImages_BookId",
                table: "BookImage",
                newName: "IX_BookImage_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookImage",
                table: "BookImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookImage_Books_BookId",
                table: "BookImage",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
