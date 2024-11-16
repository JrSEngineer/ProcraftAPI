using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingProcessTable_AddingScopeAndAbilitesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinishForecast",
                table: "Process",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ScopeId",
                table: "Process",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartForecast",
                table: "Process",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Scope",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scope", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessScopeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ability_Scope_ProcessScopeId",
                        column: x => x.ProcessScopeId,
                        principalTable: "Scope",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Process_ScopeId",
                table: "Process",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ability_ProcessScopeId",
                table: "Ability",
                column: "ProcessScopeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Process_Scope_ScopeId",
                table: "Process",
                column: "ScopeId",
                principalTable: "Scope",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Process_Scope_ScopeId",
                table: "Process");

            migrationBuilder.DropTable(
                name: "Ability");

            migrationBuilder.DropTable(
                name: "Scope");

            migrationBuilder.DropIndex(
                name: "IX_Process_ScopeId",
                table: "Process");

            migrationBuilder.DropColumn(
                name: "FinishForecast",
                table: "Process");

            migrationBuilder.DropColumn(
                name: "ScopeId",
                table: "Process");

            migrationBuilder.DropColumn(
                name: "StartForecast",
                table: "Process");
        }
    }
}
