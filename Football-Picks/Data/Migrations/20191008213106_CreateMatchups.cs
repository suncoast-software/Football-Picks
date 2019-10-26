using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Football_Picks.Data.Migrations
{
    public partial class CreateMatchups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picks_Players_PlayerId",
                table: "Picks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Picks",
                table: "Picks");

            migrationBuilder.RenameTable(
                name: "Picks",
                newName: "Pick");

            migrationBuilder.RenameIndex(
                name: "IX_Picks_PlayerId",
                table: "Pick",
                newName: "IX_Pick_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pick",
                table: "Pick",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Team_Name = table.Column<string>(nullable: true),
                    Team_Abr = table.Column<string>(nullable: true),
                    Team_Record = table.Column<string>(nullable: true),
                    Score = table.Column<string>(nullable: true),
                    Logo_Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matchups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AwayTeamId = table.Column<int>(nullable: true),
                    HomeTeamId = table.Column<int>(nullable: true),
                    Week = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matchups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matchups_Team_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matchups_Team_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matchups_AwayTeamId",
                table: "Matchups",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matchups_HomeTeamId",
                table: "Matchups",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pick_Players_PlayerId",
                table: "Pick",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pick_Players_PlayerId",
                table: "Pick");

            migrationBuilder.DropTable(
                name: "Matchups");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pick",
                table: "Pick");

            migrationBuilder.RenameTable(
                name: "Pick",
                newName: "Picks");

            migrationBuilder.RenameIndex(
                name: "IX_Pick_PlayerId",
                table: "Picks",
                newName: "IX_Picks_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picks",
                table: "Picks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Picks_Players_PlayerId",
                table: "Picks",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
