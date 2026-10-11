using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBasePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseRate",
                table: "RoomTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseRate",
                table: "RoomTypes",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
