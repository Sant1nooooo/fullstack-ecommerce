using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class OriginalandDiscountedpriceonCartProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CartProducts",
                newName: "OriginalPrice");

            migrationBuilder.AddColumn<int>(
                name: "DiscountedPrice",
                table: "CartProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "CartProducts");

            migrationBuilder.RenameColumn(
                name: "OriginalPrice",
                table: "CartProducts",
                newName: "Price");
        }
    }
}
