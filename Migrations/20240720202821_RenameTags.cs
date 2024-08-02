using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusNoti.Migrations
{
    /// <inheritdoc />
    public partial class RenameTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "Channels",
                newName: "Types");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Types",
                table: "Channels",
                newName: "Tags");
        }
    }
}
