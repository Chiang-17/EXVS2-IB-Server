using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerInfinite.Migrations;

// Upgrade from released v1.1.1 (20260530083847) directly to the v1.2.0 schema.
// Unreleased debugging schemas and selector IDs are deliberately not migration history.
public partial class UpgradeToVersion120 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "MarkerColorId",
            table: "exvs2ob_general_setting",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);

        // Stage save data was created by 20260529190428_AddStageSaveData.
        // Restore its model mapping without recreating or dropping existing saves.

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

        migrationBuilder.CreateTable(
            name: "exvs2ob_user_sub_title_1",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                TitleTextId = table.Column<uint>(type: "INTEGER", nullable: false),
                TitleOrnamentId = table.Column<uint>(type: "INTEGER", nullable: false),
                TitleEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                TitleBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                CustomText = table.Column<string>(type: "TEXT", nullable: false),
                UserTitleSettingId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_exvs2ob_user_sub_title_1", x => x.Id);
                table.ForeignKey(
                    name: "FK_exvs2ob_user_sub_title_1_exvs2ob_user_title_setting_UserTitleSettingId",
                    column: x => x.UserTitleSettingId,
                    principalTable: "exvs2ob_user_title_setting",
                    principalColumn: "UserTitleSettingId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "exvs2ob_user_sub_title_2",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                TitleTextId = table.Column<uint>(type: "INTEGER", nullable: false),
                TitleOrnamentId = table.Column<uint>(type: "INTEGER", nullable: false),
                TitleEffectId = table.Column<uint>(type: "INTEGER", nullable: false),
                TitleBackgroundPartsId = table.Column<uint>(type: "INTEGER", nullable: false),
                CustomText = table.Column<string>(type: "TEXT", nullable: false),
                UserTitleSettingId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_exvs2ob_user_sub_title_2", x => x.Id);
                table.ForeignKey(
                    name: "FK_exvs2ob_user_sub_title_2_exvs2ob_user_title_setting_UserTitleSettingId",
                    column: x => x.UserTitleSettingId,
                    principalTable: "exvs2ob_user_title_setting",
                    principalColumn: "UserTitleSettingId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_exvs2ob_user_sub_title_1_Id",
            table: "exvs2ob_user_sub_title_1",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_exvs2ob_user_sub_title_1_UserTitleSettingId",
            table: "exvs2ob_user_sub_title_1",
            column: "UserTitleSettingId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_exvs2ob_user_sub_title_2_Id",
            table: "exvs2ob_user_sub_title_2",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_exvs2ob_user_sub_title_2_UserTitleSettingId",
            table: "exvs2ob_user_sub_title_2",
            column: "UserTitleSettingId",
            unique: true);

        // Existing cards need the same one-to-one subtitle rows as newly created cards.
        migrationBuilder.Sql("""
            INSERT INTO exvs2ob_user_sub_title_1
                (UserTitleSettingId, TitleTextId, TitleOrnamentId, TitleEffectId,
                 TitleBackgroundPartsId, CustomText, CreateTime, UpdateTime)
            SELECT UserTitleSettingId, 0, 0, 0, 0, '', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
            FROM exvs2ob_user_title_setting;
            """);
        migrationBuilder.Sql("""
            INSERT INTO exvs2ob_user_sub_title_2
                (UserTitleSettingId, TitleTextId, TitleOrnamentId, TitleEffectId,
                 TitleBackgroundPartsId, CustomText, CreateTime, UpdateTime)
            SELECT UserTitleSettingId, 0, 0, 0, 0, '', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
            FROM exvs2ob_user_title_setting;
            """);

        migrationBuilder.AddColumn<uint>(
            name: "SubBackgroundId",
            table: "exvs2ob_default_sticker_profile",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0u);

        // Published 24 = Default Title; 25 = MS daily win rate and must stay 25.
        RemapDefaultTitle(migrationBuilder, 24, 100);
        RecreateRankView(migrationBuilder, "PlayerLevelId DESC, PlayerExp DESC");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        RecreateRankView(migrationBuilder, "PrestigeId DESC, PlayerLevelId DESC, PlayerExp DESC");
        RemapDefaultTitle(migrationBuilder, 100, 24);

        migrationBuilder.DropColumn(
            name: "SubBackgroundId",
            table: "exvs2ob_default_sticker_profile");

        migrationBuilder.DropTable(
            name: "exvs2ob_user_sub_title_1");

        migrationBuilder.DropTable(
            name: "exvs2ob_user_sub_title_2");

        migrationBuilder.DropTable(
            name: "exvs2ob_route_battle_high_score");

        if (ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // The bundled SQLite supports native DROP COLUMN. EF's table rebuild
            // would temporarily break the existing player-level ranking view.
            migrationBuilder.Sql("ALTER TABLE exvs2ob_battle_player_level DROP COLUMN BadgeExp;");
            migrationBuilder.Sql("ALTER TABLE exvs2ob_battle_player_level DROP COLUMN BadgeId;");
        }
        else
        {
            migrationBuilder.DropColumn(name: "BadgeExp", table: "exvs2ob_battle_player_level");
            migrationBuilder.DropColumn(name: "BadgeId", table: "exvs2ob_battle_player_level");
        }

        migrationBuilder.DropColumn(name: "MarkerColorId", table: "exvs2ob_general_setting");
    }

    private static void RemapDefaultTitle(MigrationBuilder migrationBuilder, int from, int to)
    {
        foreach (var table in new[] { "exvs2ob_default_sticker_profile", "exvs2ob_ms_sticker" })
            migrationBuilder.Sql($"""
                UPDATE {table} SET
                    Tracker1 = CASE Tracker1 WHEN {from} THEN {to} ELSE Tracker1 END,
                    Tracker2 = CASE Tracker2 WHEN {from} THEN {to} ELSE Tracker2 END,
                    Tracker3 = CASE Tracker3 WHEN {from} THEN {to} ELSE Tracker3 END;
                """);
    }

    private static void RecreateRankView(MigrationBuilder migrationBuilder, string ordering)
    {
        migrationBuilder.Sql("DROP VIEW exvs2ob_player_level_rank_view;");
        migrationBuilder.Sql($"""
            CREATE VIEW exvs2ob_player_level_rank_view AS
            SELECT DENSE_RANK() OVER (ORDER BY {ordering}) AS Rank,
                   CardProfile.UserName,
                   PlayerLevel.PrestigeId, PlayerLevel.PlayerLevelId, PlayerLevel.PlayerExp,
                   TotalWin, TotalLose
            FROM exvs2ob_battle_player_level PlayerLevel
            JOIN exvs2ob_card_profile CardProfile ON PlayerLevel.CardId = CardProfile.Id
            JOIN exvs2ob_battle_win_loss_record WinLossRecord ON PlayerLevel.CardId = WinLossRecord.CardId;
            """);
    }
}
