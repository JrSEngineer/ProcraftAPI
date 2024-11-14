using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authentication",
                columns: table => new
                {
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    AccountStatus = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authentication", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Process",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Progress = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Process", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileImage = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Cpf = table.Column<string>(type: "text", nullable: false),
                    AuthenticationEmail = table.Column<string>(type: "text", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Authentication_AuthenticationEmail",
                        column: x => x.AuthenticationEmail,
                        principalTable: "Authentication",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessUser", x => new { x.UserId, x.ProcessId });
                    table.ForeignKey(
                        name: "FK_ProcessUser_Process_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Process",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcraftProcessProcraftUser",
                columns: table => new
                {
                    ProcessesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcraftProcessProcraftUser", x => new { x.ProcessesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ProcraftProcessProcraftUser_Process_ProcessesId",
                        column: x => x.ProcessesId,
                        principalTable: "Process",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcraftProcessProcraftUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessUser_ProcessId",
                table: "ProcessUser",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcraftProcessProcraftUser_UsersId",
                table: "ProcraftProcessProcraftUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AddressId",
                table: "User",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AuthenticationEmail",
                table: "User",
                column: "AuthenticationEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessUser");

            migrationBuilder.DropTable(
                name: "ProcraftProcessProcraftUser");

            migrationBuilder.DropTable(
                name: "Process");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Authentication");
        }
    }
}
