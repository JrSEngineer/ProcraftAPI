using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatingStepActionAndStepUserTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Step",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Progress = table.Column<int>(type: "integer", nullable: false),
                    StartForecast = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishForecast = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcraftProcessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Step", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Step_Process_ProcraftProcessId",
                        column: x => x.ProcraftProcessId,
                        principalTable: "Process",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Progress = table.Column<int>(type: "integer", nullable: false),
                    StartForecast = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishForecast = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StepId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessStepId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProcraftUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Action_Step_ProcessStepId",
                        column: x => x.ProcessStepId,
                        principalTable: "Step",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Action_User_ProcraftUserId",
                        column: x => x.ProcraftUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProcessStepProcraftUser",
                columns: table => new
                {
                    StepsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessStepProcraftUser", x => new { x.StepsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ProcessStepProcraftUser_Step_StepsId",
                        column: x => x.StepsId,
                        principalTable: "Step",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessStepProcraftUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StepUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StepId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepUser", x => new { x.UserId, x.StepId });
                    table.ForeignKey(
                        name: "FK_StepUser_Step_StepId",
                        column: x => x.StepId,
                        principalTable: "Step",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StepUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_ProcessStepId",
                table: "Action",
                column: "ProcessStepId");

            migrationBuilder.CreateIndex(
                name: "IX_Action_ProcraftUserId",
                table: "Action",
                column: "ProcraftUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessStepProcraftUser_UsersId",
                table: "ProcessStepProcraftUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Step_ProcraftProcessId",
                table: "Step",
                column: "ProcraftProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_StepUser_StepId",
                table: "StepUser",
                column: "StepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropTable(
                name: "ProcessStepProcraftUser");

            migrationBuilder.DropTable(
                name: "StepUser");

            migrationBuilder.DropTable(
                name: "Step");
        }
    }
}
