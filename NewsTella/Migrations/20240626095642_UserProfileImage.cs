using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsTella.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DatePublished",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Articles");
        }
    }
}
