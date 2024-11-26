using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingActionTableProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishForecast",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "StartForecast",
                table: "Action");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Action",
                type: "interval",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Action");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishForecast",
                table: "Action",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartForecast",
                table: "Action",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
