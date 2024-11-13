using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcraftAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserAndAddressTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Address_UserAddressId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UserAddressId",
                table: "User",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_User_UserAddressId",
                table: "User",
                newName: "IX_User_AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Address_AddressId",
                table: "User",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Address_AddressId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "User",
                newName: "UserAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_User_AddressId",
                table: "User",
                newName: "IX_User_UserAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Address_UserAddressId",
                table: "User",
                column: "UserAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
