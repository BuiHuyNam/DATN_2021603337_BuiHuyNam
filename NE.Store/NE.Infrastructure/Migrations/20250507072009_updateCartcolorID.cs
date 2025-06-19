using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCartcolorID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Cart");
        }
    }
}
