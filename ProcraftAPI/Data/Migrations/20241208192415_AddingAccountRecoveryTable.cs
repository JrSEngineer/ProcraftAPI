using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingAccountRecoveryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SendedAt",
                table: "Recovery",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendedAt",
                table: "Recovery");
        }
    }
}
