using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class OrderLineAddedBookPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOrdered",
                table: "Orders",
                newName: "DateOrderedUtc");

            migrationBuilder.AddColumn<decimal>(
                name: "BookPrice",
                table: "OrderLineItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookPrice",
                table: "OrderLineItem");

            migrationBuilder.RenameColumn(
                name: "DateOrderedUtc",
                table: "Orders",
                newName: "DateOrdered");
        }
    }
}
