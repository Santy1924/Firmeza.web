using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firmeza.web.Migrations
{
    /// <inheritdoc />
    public partial class deletecampoactividad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Productos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
