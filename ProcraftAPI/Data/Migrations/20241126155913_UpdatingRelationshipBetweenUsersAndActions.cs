using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingRelationshipBetweenUsersAndActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_User_ProcraftUserId",
                table: "Action");

            migrationBuilder.DropIndex(
                name: "IX_Action_ProcraftUserId",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "ProcraftUserId",
                table: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_Action_UserId",
                table: "Action",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_User_UserId",
                table: "Action",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_User_UserId",
                table: "Action");

            migrationBuilder.DropIndex(
                name: "IX_Action_UserId",
                table: "Action");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcraftUserId",
                table: "Action",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Action_ProcraftUserId",
                table: "Action",
                column: "ProcraftUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_User_ProcraftUserId",
                table: "Action",
                column: "ProcraftUserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
