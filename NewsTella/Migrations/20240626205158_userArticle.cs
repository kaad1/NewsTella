using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsTella.Migrations
{
    /// <inheritdoc />
    public partial class userArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<DateTime>(
            //    name: "DatePublished",
            //    table: "Articles",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsArchived",
            //    table: "Articles",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "UserId",
            //    table: "Articles",
            //    type: "nvarchar(450)",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Articles_UserId",
            //    table: "Articles",
            //    column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_UserId",
                table: "Articles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_UserId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_UserId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "DatePublished",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Articles");
        }
    }
}
