using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFolioIsMainFolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMainFolio",
                table: "Folios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(
                """
                UPDATE f
                SET f.IsMainFolio = 1
                FROM Folios f
                INNER JOIN
                (
                    SELECT FiscalAccountId, MIN(CreatedAt) AS EarliestCreatedAt
                    FROM Folios
                    GROUP BY FiscalAccountId
                ) earliest ON earliest.FiscalAccountId = f.FiscalAccountId
                    AND earliest.EarliestCreatedAt = f.CreatedAt
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMainFolio",
                table: "Folios");
        }
    }
}
