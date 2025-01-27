using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsLetterBanan.Migrations
{
    /// <inheritdoc />
    public partial class addededitorsChoiseToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsEditorsChoice",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditorsChoice",
                table: "Articles");
        }
    }
}
