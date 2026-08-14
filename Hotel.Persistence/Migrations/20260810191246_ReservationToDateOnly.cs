using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReservationToDateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add nullable date columns first
            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDateDate",
                table: "Reservations",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDateDate",
                table: "Reservations",
                type: "date",
                nullable: true);

            // Copy data from old datetime2 columns to new date columns
            migrationBuilder.Sql(
                "UPDATE Reservations SET StartDateDate = CAST(StartDate AS date), EndDateDate = CAST(EndDate AS date)");

            // Drop old datetime2 columns
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Reservations");

            // Rename new date columns to original names
            migrationBuilder.RenameColumn(
                name: "StartDateDate",
                table: "Reservations",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndDateDate",
                table: "Reservations",
                newName: "EndDate");

            // Make columns non-nullable after all data has been populated
            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "Reservations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "Reservations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add nullable datetime2 columns
            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateDt",
                table: "Reservations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateDt",
                table: "Reservations",
                type: "datetime2",
                nullable: true);

            // Copy data from date columns to datetime2 columns
            migrationBuilder.Sql(
                "UPDATE Reservations SET StartDateDt = CAST(StartDate AS datetime2), EndDateDt = CAST(EndDate AS datetime2)");

            // Drop date columns
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Reservations");

            // Rename datetime2 columns to original names
            migrationBuilder.RenameColumn(
                name: "StartDateDt",
                table: "Reservations",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndDateDt",
                table: "Reservations",
                newName: "EndDate");

            // Make columns non-nullable
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}