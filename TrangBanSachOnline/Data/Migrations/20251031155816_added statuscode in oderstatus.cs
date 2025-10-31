using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrangBanSachOnline.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedstatuscodeinoderstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "OderStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OderStatus");
        }
    }
}
