using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electronic_Bank.Migrations
{
    public partial class initSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientImg",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientImg",
                table: "Clients");
        }
    }
}
