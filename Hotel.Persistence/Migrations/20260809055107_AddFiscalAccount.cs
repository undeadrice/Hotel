using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFiscalAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Folios");

            migrationBuilder.RenameColumn(
                name: "OriginatorId",
                table: "Folios",
                newName: "FiscalAccountId");

            migrationBuilder.CreateTable(
                name: "FiscalAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalAccounts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folios_FiscalAccountId",
                table: "Folios",
                column: "FiscalAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folios_FiscalAccounts_FiscalAccountId",
                table: "Folios",
                column: "FiscalAccountId",
                principalTable: "FiscalAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folios_FiscalAccounts_FiscalAccountId",
                table: "Folios");

            migrationBuilder.DropTable(
                name: "FiscalAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Folios_FiscalAccountId",
                table: "Folios");

            migrationBuilder.RenameColumn(
                name: "FiscalAccountId",
                table: "Folios",
                newName: "OriginatorId");

            migrationBuilder.AddColumn<Guid>(
                name: "GuestId",
                table: "Folios",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
