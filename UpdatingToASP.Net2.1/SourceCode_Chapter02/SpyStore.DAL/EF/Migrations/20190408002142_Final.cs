using Microsoft.EntityFrameworkCore.Migrations;

namespace SpyStore.DAL.EF.Migrations
{
    public partial class Final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "OrderTotal",
                schema: "Store",
                table: "Orders",
                type: "money",
                nullable: true,
                computedColumnSql: "Store.GetOrderTotal([Id])",
                oldClrType: typeof(decimal),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "OrderTotal",
                schema: "Store",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true,
                oldComputedColumnSql: "Store.GetOrderTotal([Id])");
        }
    }
}
