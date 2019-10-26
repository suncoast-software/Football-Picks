using Microsoft.EntityFrameworkCore.Migrations;

namespace Football_Picks.Data.Migrations
{
    public partial class UpdatePick : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TieBreaker",
                table: "Pick",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TieBreaker",
                table: "Pick");
        }
    }
}
