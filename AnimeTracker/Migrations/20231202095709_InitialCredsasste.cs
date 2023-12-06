using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCredsasste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnimeId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AnimeId",
                table: "Comment",
                column: "AnimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Animes_AnimeId",
                table: "Comment",
                column: "AnimeId",
                principalTable: "Animes",
                principalColumn: "AnimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Animes_AnimeId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_AnimeId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "AnimeId",
                table: "Comment");
        }
    }
}
