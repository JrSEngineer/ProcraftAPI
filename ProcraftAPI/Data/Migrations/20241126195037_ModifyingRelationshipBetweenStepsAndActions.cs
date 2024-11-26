using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyingRelationshipBetweenStepsAndActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_Step_ProcessStepId",
                table: "Action");

            migrationBuilder.DropIndex(
                name: "IX_Action_ProcessStepId",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "ProcessStepId",
                table: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_Action_StepId",
                table: "Action",
                column: "StepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Step_StepId",
                table: "Action",
                column: "StepId",
                principalTable: "Step",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_Step_StepId",
                table: "Action");

            migrationBuilder.DropIndex(
                name: "IX_Action_StepId",
                table: "Action");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcessStepId",
                table: "Action",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Action_ProcessStepId",
                table: "Action",
                column: "ProcessStepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Step_ProcessStepId",
                table: "Action",
                column: "ProcessStepId",
                principalTable: "Step",
                principalColumn: "Id");
        }
    }
}
