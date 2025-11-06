using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrangBanSachOnline.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameOderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oder_OderStatus_OderStatusId",
                table: "Oder");

            migrationBuilder.DropForeignKey(
                name: "FK_OderDetail_Book_BookId",
                table: "OderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OderDetail_Oder_OderId",
                table: "OderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OderStatus",
                table: "OderStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OderDetail",
                table: "OderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Oder",
                table: "Oder");

            migrationBuilder.RenameTable(
                name: "OderStatus",
                newName: "OrderStatus");

            migrationBuilder.RenameTable(
                name: "OderDetail",
                newName: "OrderDetail");

            migrationBuilder.RenameTable(
                name: "Oder",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_OderDetail_OderId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_OderId");

            migrationBuilder.RenameIndex(
                name: "IX_OderDetail_BookId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Oder_OderStatusId",
                table: "Order",
                newName: "IX_Order_OderStatusId");

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "CartDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderStatus",
                table: "OrderStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderStatus_OderStatusId",
                table: "Order",
                column: "OderStatusId",
                principalTable: "OrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Book_BookId",
                table: "OrderDetail",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_OderId",
                table: "OrderDetail",
                column: "OderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderStatus_OderStatusId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Book_BookId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_OderId",
                table: "OrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderStatus",
                table: "OrderStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDetail");

            migrationBuilder.RenameTable(
                name: "OrderStatus",
                newName: "OderStatus");

            migrationBuilder.RenameTable(
                name: "OrderDetail",
                newName: "OderDetail");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Oder");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_OderId",
                table: "OderDetail",
                newName: "IX_OderDetail_OderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_BookId",
                table: "OderDetail",
                newName: "IX_OderDetail_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_OderStatusId",
                table: "Oder",
                newName: "IX_Oder_OderStatusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OderStatus",
                table: "OderStatus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OderDetail",
                table: "OderDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Oder",
                table: "Oder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Oder_OderStatus_OderStatusId",
                table: "Oder",
                column: "OderStatusId",
                principalTable: "OderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OderDetail_Book_BookId",
                table: "OderDetail",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OderDetail_Oder_OderId",
                table: "OderDetail",
                column: "OderId",
                principalTable: "Oder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
