using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimeTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCredaste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "UserAnimeLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "UserAnimeLists");
        }
    }
}
