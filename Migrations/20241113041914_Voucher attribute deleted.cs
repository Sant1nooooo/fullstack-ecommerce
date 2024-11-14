using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class Voucherattributedeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_Vouchers_VoucherID",
                table: "CartProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_VoucherID",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "CartProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoucherID",
                table: "CartProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_VoucherID",
                table: "CartProducts",
                column: "VoucherID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_Vouchers_VoucherID",
                table: "CartProducts",
                column: "VoucherID",
                principalTable: "Vouchers",
                principalColumn: "ID");
        }
    }
}
