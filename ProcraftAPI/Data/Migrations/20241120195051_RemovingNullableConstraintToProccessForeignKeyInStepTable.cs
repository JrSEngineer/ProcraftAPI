using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovingNullableConstraintToProccessForeignKeyInStepTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Step_Process_ProcraftProcessId",
                table: "Step");

            migrationBuilder.AddForeignKey(
                name: "FK_Step_Process_ProcraftProcessId",
                table: "Step",
                column: "ProcraftProcessId",
                principalTable: "Process",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Step_Process_ProcraftProcessId",
                table: "Step");

            migrationBuilder.AddForeignKey(
                name: "FK_Step_Process_ProcraftProcessId",
                table: "Step",
                column: "ProcraftProcessId",
                principalTable: "Process",
                principalColumn: "Id");
        }
    }
}
