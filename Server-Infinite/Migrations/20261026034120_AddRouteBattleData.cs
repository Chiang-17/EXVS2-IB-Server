using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerInfinite.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteBattleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // NOTE: exvs2ob_stage_save_data already exists (migration 20260529190428_AddStageSaveData);
            // only the EF model/snapshot entry is added here.

            migrationBuilder.AddColumn<int>(
                name: "BadgeExp",
                table: "exvs2ob_battle_player_level",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<uint>(
                name: "BadgeId",
                table: "exvs2ob_battle_player_level",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.CreateTable(
                name: "exvs2ob_route_battle_high_score",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Difficulty = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearTime1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearScore1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearTime2 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearScore2 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearTime3 = table.Column<uint>(type: "INTEGER", nullable: false),
                    ClearScore3 = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalClearTime = table.Column<uint>(type: "INTEGER", nullable: false),
                    TotalClearScore = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exvs2ob_route_battle_high_score", x => x.Id);
                    table.ForeignKey(
                        name: "FK_exvs2ob_route_battle_high_score_exvs2ob_card_profile_CardId",
                        column: x => x.CardId,
                        principalTable: "exvs2ob_card_profile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_route_battle_high_score_CardId_Difficulty",
                table: "exvs2ob_route_battle_high_score",
                columns: new[] { "CardId", "Difficulty" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_exvs2ob_route_battle_high_score_Id",
                table: "exvs2ob_route_battle_high_score",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exvs2ob_route_battle_high_score");

            migrationBuilder.DropColumn(
                name: "BadgeExp",
                table: "exvs2ob_battle_player_level");

            migrationBuilder.DropColumn(
                name: "BadgeId",
                table: "exvs2ob_battle_player_level");
        }
    }
}
