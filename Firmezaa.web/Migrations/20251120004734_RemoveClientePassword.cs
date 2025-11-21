using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firmeza.web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClientePassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Clientes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Clientes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Clientes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
