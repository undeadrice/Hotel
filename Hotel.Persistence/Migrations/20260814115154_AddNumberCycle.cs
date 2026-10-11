using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CycleIdentifier",
                table: "Reservations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "None");

            migrationBuilder.AddColumn<string>(
                name: "CycleIdentifier",
                table: "FiscalAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "None");

            migrationBuilder.CreateTable(
                name: "NumberCycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Topic = table.Column<int>(type: "int", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartIndex = table.Column<int>(type: "int", nullable: false),
                    CurrentIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberCycles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NumberCycles_Topic",
                table: "NumberCycles",
                column: "Topic",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumberCycles");

            migrationBuilder.DropColumn(
                name: "CycleIdentifier",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CycleIdentifier",
                table: "FiscalAccounts");
        }
    }
}
