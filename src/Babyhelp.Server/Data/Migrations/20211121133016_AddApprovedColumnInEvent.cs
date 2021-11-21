using Microsoft.EntityFrameworkCore.Migrations;

namespace Babyhelp.Server.Data.Migrations
{
    public partial class AddApprovedColumnInEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Events");
        }
    }
}
