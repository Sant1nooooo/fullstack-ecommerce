using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class AppliedVouchers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppliedVouchers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartProductID = table.Column<int>(type: "int", nullable: true),
                    VoucherID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedVouchers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AppliedVouchers_CartProducts_CartProductID",
                        column: x => x.CartProductID,
                        principalTable: "CartProducts",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AppliedVouchers_Vouchers_VoucherID",
                        column: x => x.VoucherID,
                        principalTable: "Vouchers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppliedVouchers_CartProductID",
                table: "AppliedVouchers",
                column: "CartProductID");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedVouchers_VoucherID",
                table: "AppliedVouchers",
                column: "VoucherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppliedVouchers");
        }
    }
}
