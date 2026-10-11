using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TransactionAdjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TransactionGroups");

            migrationBuilder.DropColumn(
                name: "DefaultAmount",
                table: "TransactionCodes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TransactionCodes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TransactionGroups",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultAmount",
                table: "TransactionCodes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TransactionCodes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
