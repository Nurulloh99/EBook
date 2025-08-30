using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBook.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ThumbnaliAddedToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnaliUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnaliUrl",
                table: "Books");
        }
    }
}
