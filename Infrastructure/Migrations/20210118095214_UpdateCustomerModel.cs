using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class UpdateCustomerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$12$96xrUHr3ZvKTXYHRAX0SHudg7D4TwP85B2yR7EZqJ9OoBfPy.ovJa");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$12$4GxR5siwunCkOkh64XnNMOQhTG2yuK0FC4oVz10jhPZzxIXDoQQy.");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "EmailAddress", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "admin@lotus.nl", "$2a$12$iYOTFRWCkzDhZ6n9.n96FO.DWzutNygdhTqtaECqvIxzWDlTZVzCS", 5, "Admin" },
                    { 2, "jeff@memberson.nl", "$2a$12$sBCEpDWEuxJrdhuunScodeQzjqp8lq/kck8Z8LhtwoSbseeldnqo6", 1, "Jeff Memberson" },
                    { 3, "pad.jetty@okayvoorzee.nl", "$2a$12$p4gEha4LIX8RKkoBs2dz/eaDJj88t2eVvADpsBK0hS5rYhbhTMoz6", 2, "Pad Jetty" },
                    { 4, "sensu.overdijk@hotmail.nl", "$2a$12$sIwOmLEWrb9sJsrejfkP.eRh2bTrc8SE8FMPNHV4xDxXpTvqYdG8a", 3, "Sensu Overdijk" },
                    { 5, "bill.versteen@zig.nl", "$2a$12$vnDlJGZxlNiRwRWM6cXMRuci9N4Cc4URNH/9qYZxqsg6tSi0OPG/K", 4, "Bill Versteen" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_EmailAddress",
                table: "Customers",
                column: "EmailAddress",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_EmailAddress",
                table: "Customers");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Customers");
        }
    }
}
