using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowerSite.Migrations
{
    public partial class ChangeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Bios",
                newName: "Footerlogo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Footerlogo",
                table: "Bios",
                newName: "Logo");
        }
    }
}
