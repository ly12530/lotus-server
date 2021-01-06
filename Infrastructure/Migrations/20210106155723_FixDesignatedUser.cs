using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class FixDesignatedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Customers_CustomerId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_UserId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Requests",
                newName: "DesignatedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_UserId",
                table: "Requests",
                newName: "IX_Requests_DesignatedUserId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Requests",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Customers_CustomerId",
                table: "Requests",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_DesignatedUserId",
                table: "Requests",
                column: "DesignatedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Customers_CustomerId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_DesignatedUserId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "DesignatedUserId",
                table: "Requests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_DesignatedUserId",
                table: "Requests",
                newName: "IX_Requests_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Requests",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Customers_CustomerId",
                table: "Requests",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_UserId",
                table: "Requests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
