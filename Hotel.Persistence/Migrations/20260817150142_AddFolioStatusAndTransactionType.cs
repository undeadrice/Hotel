using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFolioStatusAndTransactionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Folios",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "FolioItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                """
                UPDATE fi
                SET fi.TransactionType = tg.Type
                FROM FolioItems fi
                INNER JOIN TransactionCodes tc ON tc.Id = fi.TransactionCodeId
                INNER JOIN TransactionGroups tg ON tg.Id = tc.TransactionGroupId
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Folios");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "FolioItems");
        }
    }
}
