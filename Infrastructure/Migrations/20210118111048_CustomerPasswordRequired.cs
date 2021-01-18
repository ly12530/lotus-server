using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class CustomerPasswordRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$12$f/5501xRea8XkLeVzmp28ObnFaa/7qQIsXfGIVJLwsIWVNWiP/o4a");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$12$Yk8sFwVt2YY3NwLIqwCAkO1HAKVTp7O7XTmLV028.iJBKfxskA6Wq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$12$2TB8pF4ymQNx5AIAzeLZ7OC3VKkTI97XuwUiaeA5e4kGq8sK/m8l2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$12$H6nt59pGYF01Xa/BBV.FXOTwmpbBmclNHjeyTmV9TZhrJQJpHqwfK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$12$0ZekQrCzeKZ563pEbWKDOOZjTNHy6XzWJuMn1It3ZVG7l6PG9NQK6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$12$pvfdPNJL611bN/dkqkG76OJrCY7Ddxck4eX6wdLN0w4iIhwB72Sli");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$12$tXlwgK3aumtqxXzqHwb08OhXdxqsnm2xHn29abssheg8vLFLoGgve");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$12$iYOTFRWCkzDhZ6n9.n96FO.DWzutNygdhTqtaECqvIxzWDlTZVzCS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "$2a$12$sBCEpDWEuxJrdhuunScodeQzjqp8lq/kck8Z8LhtwoSbseeldnqo6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Password",
                value: "$2a$12$p4gEha4LIX8RKkoBs2dz/eaDJj88t2eVvADpsBK0hS5rYhbhTMoz6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "$2a$12$sIwOmLEWrb9sJsrejfkP.eRh2bTrc8SE8FMPNHV4xDxXpTvqYdG8a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Password",
                value: "$2a$12$vnDlJGZxlNiRwRWM6cXMRuci9N4Cc4URNH/9qYZxqsg6tSi0OPG/K");
        }
    }
}
