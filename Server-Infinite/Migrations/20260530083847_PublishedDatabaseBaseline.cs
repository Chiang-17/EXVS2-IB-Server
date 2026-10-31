using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerInfinite.Migrations;

// Fresh-database baseline for the canonical v1.1.x schema. Published v1.0
// databases share the checkpoint but may retain six unused tracker-text columns.
// The ORIGINAL published checkpoint ID is intentionally retained in the Designer:
// existing users already have it in __EFMigrationsHistory and must skip this initializer.
// Older individual history rows remain in user databases; they are never rewritten.
public partial class PublishedDatabaseBaseline : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_battle_action_log" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_battle_action_log" PRIMARY KEY AUTOINCREMENT,
                "BattleHistoryId" INTEGER NOT NULL,
                "CardId" INTEGER NOT NULL,
                "FrameTime" INTEGER NOT NULL,
                "PlayerName" TEXT NOT NULL,
                "ActionType" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_battle_action_log_exvs2ob_battle_history_BattleHistoryId" FOREIGN KEY ("BattleHistoryId") REFERENCES "exvs2ob_battle_history" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_battle_ally" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_battle_ally" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "BattleHistoryId" INTEGER NOT NULL,
                "CardId" INTEGER NOT NULL,
                "PlayerName" TEXT NOT NULL,
                "ClassId" INTEGER NOT NULL,
                "PrestigeId" INTEGER NOT NULL,
                "LevelId" INTEGER NOT NULL,
                "MobileSuitId" INTEGER NOT NULL,
                "SkinId" INTEGER NOT NULL,
                "Mastery" INTEGER NOT NULL,
                "BurstType" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_battle_ally_exvs2ob_battle_history_BattleHistoryId" FOREIGN KEY ("BattleHistoryId") REFERENCES "exvs2ob_battle_history" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_battle_history" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_battle_history" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "BattleMode" TEXT NOT NULL,
                "IsWin" INTEGER NOT NULL,
                "PlayedAt" INTEGER NOT NULL,
                "ElapsedSeconds" INTEGER NOT NULL,
                "TeamId" INTEGER NOT NULL,
                "StageId" INTEGER NOT NULL,
                "Score" INTEGER NOT NULL,
                "ScoreRank" INTEGER NOT NULL,
                "BurstType" INTEGER NOT NULL,
                "BurstCount" INTEGER NOT NULL,
                "TotalExBurstDamage" INTEGER NOT NULL,
                "GivenDamage" INTEGER NOT NULL,
                "TakenDamage" INTEGER NOT NULL,
                "OverheatCount" INTEGER NOT NULL,
                "ComboGivenDamage" INTEGER NOT NULL,
                "ConsecutiveWinCount" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_battle_history_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_battle_player_level" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_battle_player_level" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "PlayerLevelId" INTEGER NOT NULL,
                "PrestigeId" INTEGER NOT NULL,
                "PlayerExp" INTEGER NOT NULL,
                "LevelMaxDispFlag" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_battle_player_level_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_battle_self" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_battle_self" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "BattleHistoryId" INTEGER NOT NULL,
                "CardId" INTEGER NOT NULL,
                "PlayerName" TEXT NOT NULL,
                "ClassId" INTEGER NOT NULL,
                "PrestigeId" INTEGER NOT NULL,
                "LevelId" INTEGER NOT NULL,
                "MobileSuitId" INTEGER NOT NULL,
                "SkinId" INTEGER NOT NULL,
                "Mastery" INTEGER NOT NULL,
                "BurstType" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_battle_self_exvs2ob_battle_history_BattleHistoryId" FOREIGN KEY ("BattleHistoryId") REFERENCES "exvs2ob_battle_history" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_battle_solo_class_record" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_battle_solo_class_record" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "CardId" INTEGER NOT NULL,
                "ClassId" INTEGER NOT NULL,
                "Rate" REAL NOT NULL,
                "MaxPosition" INTEGER NOT NULL,
                "ClassChangeStatus" INTEGER NOT NULL,
                "WeeklyTotalBattleCount" INTEGER NOT NULL,
                "WeeklyTotalWinCount" INTEGER NOT NULL,
                "TopPointRankEntryCount" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_battle_solo_class_record_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_battle_target" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_battle_target" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "BattleHistoryId" INTEGER NOT NULL,
                "CardId" INTEGER NOT NULL,
                "PlayerName" TEXT NOT NULL,
                "ClassId" INTEGER NOT NULL,
                "PrestigeId" INTEGER NOT NULL,
                "LevelId" INTEGER NOT NULL,
                "MobileSuitId" INTEGER NOT NULL,
                "SkinId" INTEGER NOT NULL,
                "Mastery" INTEGER NOT NULL,
                "BurstType" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_battle_target_exvs2ob_battle_history_BattleHistoryId" FOREIGN KEY ("BattleHistoryId") REFERENCES "exvs2ob_battle_history" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_battle_team_class_record" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_battle_team_class_record" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "CardId" INTEGER NOT NULL,
                "ClassId" INTEGER NOT NULL,
                "Rate" REAL NOT NULL,
                "MaxPosition" INTEGER NOT NULL,
                "ClassChangeStatus" INTEGER NOT NULL,
                "WeeklyTotalBattleCount" INTEGER NOT NULL,
                "WeeklyTotalWinCount" INTEGER NOT NULL,
                "TopPointRankEntryCount" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_battle_team_class_record_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_battle_win_loss_record" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_battle_win_loss_record" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "TotalWin" INTEGER NOT NULL,
                "TotalLose" INTEGER NOT NULL,
                "ShuffleWin" INTEGER NOT NULL,
                "ShuffleLose" INTEGER NOT NULL,
                "TeamWin" INTEGER NOT NULL,
                "TeamLose" INTEGER NOT NULL,
                "ClassSoloWin" INTEGER NOT NULL,
                "ClassSoloLose" INTEGER NOT NULL,
                "ClassTeamWin" INTEGER NOT NULL,
                "ClassTeamLose" INTEGER NOT NULL,
                "FesWin" INTEGER NOT NULL,
                "FesLose" INTEGER NOT NULL,
                "FreeWin" INTEGER NOT NULL,
                "FreeLose" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_battle_win_loss_record_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_boost_setting" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_boost_setting" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "GpBoost" INTEGER NOT NULL,
                "GuestNavBoost" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_boost_setting_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_card_profile" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_card_profile" PRIMARY KEY AUTOINCREMENT,
                "UserName" TEXT NOT NULL,
                "Gp" INTEGER NOT NULL,
                "AccessCode" TEXT NOT NULL,
                "ChipId" TEXT NOT NULL,
                "SessionId" TEXT NOT NULL,
                "IsNewCard" INTEGER NOT NULL,
                "UploadToken" TEXT NOT NULL,
                "UploadTokenExpiry" TEXT NOT NULL,
                "DistinctTeamFormationToken" TEXT NOT NULL,
                "LastPlayedAt" INTEGER NOT NULL,
                "LastLoginCabinet" TEXT NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_challenge_mission_profile" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_challenge_mission_profile" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "EffectiveYear" INTEGER NOT NULL,
                "EffectiveMonth" INTEGER NOT NULL,
                "EffectiveDay" INTEGER NOT NULL,
                "TotalBattleCount" INTEGER NOT NULL,
                "TotalBattleWinCount" INTEGER NOT NULL,
                "MaxConsecutiveWinCount" INTEGER NOT NULL,
                "TotalDefeatCount" INTEGER NOT NULL,
                "TotalDamageCount" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_challenge_mission_profile_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_command_message_data" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_command_message_data" PRIMARY KEY AUTOINCREMENT,
                "MessageSettingId" INTEGER NOT NULL,
                "Mode" INTEGER NOT NULL,
                "StartMessageText" TEXT NOT NULL,
                "StartMessageId" INTEGER NOT NULL,
                "UpMessageText" TEXT NOT NULL,
                "UpMessageId" INTEGER NOT NULL,
                "DownMessageText" TEXT NOT NULL,
                "DownMessageId" INTEGER NOT NULL,
                "LeftMessageText" TEXT NOT NULL,
                "LeftMessageId" INTEGER NOT NULL,
                "RightMessageText" TEXT NOT NULL,
                "RightMessageId" INTEGER NOT NULL,
                "EndMessageText" TEXT NOT NULL,
                "EndMessageId" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_command_message_data_exvs2ob_message_setting_MessageSettingId" FOREIGN KEY ("MessageSettingId") REFERENCES "exvs2ob_message_setting" ("MessageSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_customize_profile" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_customize_profile" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "DefaultGaugeDesignId" INTEGER NOT NULL,
                "DefaultBgmSettings" TEXT NOT NULL,
                "DefaultBgmPlayMethod" INTEGER NOT NULL,
                "StageRandoms" TEXT NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL, "BgmSafeMode" INTEGER NOT NULL DEFAULT 0,
                CONSTRAINT "FK_exvs2ob_customize_profile_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_default_sticker_profile" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_default_sticker_profile" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "BasePanelId" INTEGER NOT NULL,
                "CommentPartsAId" INTEGER NOT NULL,
                "CommentPartsBId" INTEGER NOT NULL,
                "StickerBackgroundId" INTEGER NOT NULL,
                "StickerEffectId" INTEGER NOT NULL,
                "Tracker1" INTEGER NOT NULL,
                "Tracker2" INTEGER NOT NULL,
                "Tracker3" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL, "CutinImageUseFlag" INTEGER NOT NULL DEFAULT 0, "PilotImageDispFlag" INTEGER NOT NULL DEFAULT 0,
                CONSTRAINT "FK_exvs2ob_default_sticker_profile_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_favourite_ms" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_favourite_ms" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "MstMobileSuitId" INTEGER NOT NULL,
                "OpenSkillpoint" INTEGER NOT NULL,
                "GaugeDesignId" INTEGER NOT NULL,
                "BgmSettings" TEXT NOT NULL,
                "BgmPlayMethod" INTEGER NOT NULL,
                "BattleNavId" INTEGER NOT NULL,
                "BurstType" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_favourite_ms_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_gamepad_setting" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_gamepad_setting" PRIMARY KEY AUTOINCREMENT,
                "AKey" INTEGER NOT NULL,
                "BKey" INTEGER NOT NULL,
                "CardId" INTEGER NOT NULL,
                "CommandDispConfig" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "GamePadStyle" INTEGER NOT NULL,
                "LbKey" INTEGER NOT NULL,
                "LsbKey" INTEGER NOT NULL,
                "LtKey" INTEGER NOT NULL,
                "RbKey" INTEGER NOT NULL,
                "RsbKey" INTEGER NOT NULL,
                "RtKey" INTEGER NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "XKey" INTEGER NOT NULL,
                "YKey" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_gamepad_setting_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_general_setting" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_general_setting" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "FixPositionRadar" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL, "EnableQuickStart" INTEGER NOT NULL DEFAULT 0,
                CONSTRAINT "FK_exvs2ob_general_setting_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_invalid_visit" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_invalid_visit" PRIMARY KEY AUTOINCREMENT,
                "Token" TEXT NOT NULL,
                "Ip" TEXT NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_license_score_record" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_license_score_record" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "LicenseScore" INTEGER NOT NULL,
                "LastObtainedScore" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_license_score_record_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_message_setting" (
                "MessageSettingId" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_message_setting" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "MessagePosition" INTEGER NOT NULL,
                "AllowReceiveMessage" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL, "CommandMessageAutoFlag" INTEGER NOT NULL DEFAULT 0,
                CONSTRAINT "FK_exvs2ob_message_setting_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_ms_class_match_title" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_ms_class_match_title" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "TitleTextId" INTEGER NOT NULL,
                "TitleOrnamentId" INTEGER NOT NULL,
                "TitleEffectId" INTEGER NOT NULL,
                "TitleBackgroundPartsId" INTEGER NOT NULL,
                "CustomText" TEXT NOT NULL,
                "FavouriteMobileSuitId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_ms_class_match_title_exvs2ob_favourite_ms_FavouriteMobileSuitId" FOREIGN KEY ("FavouriteMobileSuitId") REFERENCES "exvs2ob_favourite_ms" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_ms_default_title" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_ms_default_title" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "TitleTextId" INTEGER NOT NULL,
                "TitleOrnamentId" INTEGER NOT NULL,
                "TitleEffectId" INTEGER NOT NULL,
                "TitleBackgroundPartsId" INTEGER NOT NULL,
                "CustomText" TEXT NOT NULL,
                "FavouriteMobileSuitId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_ms_default_title_exvs2ob_favourite_ms_FavouriteMobileSuitId" FOREIGN KEY ("FavouriteMobileSuitId") REFERENCES "exvs2ob_favourite_ms" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_ms_pvp_stat" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_ms_pvp_stat" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "MstMobileSuitId" INTEGER NOT NULL,
                "TotalBattleCount" INTEGER NOT NULL,
                "TotalWinCount" INTEGER NOT NULL,
                "TotalGivenDamage" INTEGER NOT NULL,
                "TotalEnemyDefeatedCount" INTEGER NOT NULL,
                "TotalClassMatchTenConsecutiveWinCount" INTEGER NOT NULL,
                "TotalNoDamageBattleCount" INTEGER NOT NULL,
                "TotalExBurstDamage" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_ms_pvp_stat_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_ms_sticker" (
                "StickerId" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_ms_sticker" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "MstMobileSuitId" INTEGER NOT NULL,
                "PoseId" INTEGER NOT NULL,
                "StickerBackgroundId" INTEGER NOT NULL,
                "StickerEffectId" INTEGER NOT NULL,
                "Tracker1" INTEGER NOT NULL,
                "Tracker2" INTEGER NOT NULL,
                "Tracker3" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL, "CostumeIds" TEXT NOT NULL DEFAULT '', "CutinImageUseFlag" INTEGER NOT NULL DEFAULT 0, "PilotImageDispFlag" INTEGER NOT NULL DEFAULT 0, "PoseIds" TEXT NOT NULL DEFAULT '', "SubBackgroundId" INTEGER NOT NULL DEFAULT 0,
                CONSTRAINT "FK_exvs2ob_ms_sticker_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_ms_triad_title" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_ms_triad_title" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "TitleTextId" INTEGER NOT NULL,
                "TitleOrnamentId" INTEGER NOT NULL,
                "TitleEffectId" INTEGER NOT NULL,
                "TitleBackgroundPartsId" INTEGER NOT NULL,
                "CustomText" TEXT NOT NULL,
                "FavouriteMobileSuitId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_ms_triad_title_exvs2ob_favourite_ms_FavouriteMobileSuitId" FOREIGN KEY ("FavouriteMobileSuitId") REFERENCES "exvs2ob_favourite_ms" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_ms_usage" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_ms_usage" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "MstMobileSuitId" INTEGER NOT NULL,
                "MsUsedNum" INTEGER NOT NULL,
                "CostumeId" INTEGER NOT NULL,
                "TriadBuddyPoint" INTEGER NOT NULL,
                "SkinId" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_ms_usage_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_navi" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_navi" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "GuestNavId" INTEGER NOT NULL,
                "GuestNavSettingFlag" INTEGER NOT NULL,
                "GuestNavRemains" INTEGER NOT NULL,
                "BattleNavSettingFlag" INTEGER NOT NULL,
                "BattleNavRemains" INTEGER NOT NULL,
                "GuestNavCostume" INTEGER NOT NULL,
                "GuestNavFamiliarity" INTEGER NOT NULL,
                "NewCostumeFlag" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_navi_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_navi_setting" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_navi_setting" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "BattleNavAdviseFlag" INTEGER NOT NULL,
                "BattleNavNotifyFlag" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_navi_setting_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_online_pair" (
                "PairId" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_online_pair" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "TeamId" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_online_pair_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_online_shuffle_opening_message" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_online_shuffle_opening_message" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "MessageSettingId" INTEGER NOT NULL,
                "TopMessageText" TEXT NOT NULL,
                "TopUniqueMessageId" INTEGER NOT NULL,
                "DownMessageText" TEXT NOT NULL,
                "DownUniqueMessageId" INTEGER NOT NULL,
                "LeftMessageText" TEXT NOT NULL,
                "LeftUniqueMessageId" INTEGER NOT NULL,
                "RightMessageText" TEXT NOT NULL,
                "RightUniqueMessageId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_online_shuffle_opening_message_exvs2ob_message_setting_MessageSettingId" FOREIGN KEY ("MessageSettingId") REFERENCES "exvs2ob_message_setting" ("MessageSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_online_shuffle_playing_message" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_online_shuffle_playing_message" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "MessageSettingId" INTEGER NOT NULL,
                "TopMessageText" TEXT NOT NULL,
                "TopUniqueMessageId" INTEGER NOT NULL,
                "DownMessageText" TEXT NOT NULL,
                "DownUniqueMessageId" INTEGER NOT NULL,
                "LeftMessageText" TEXT NOT NULL,
                "LeftUniqueMessageId" INTEGER NOT NULL,
                "RightMessageText" TEXT NOT NULL,
                "RightUniqueMessageId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_online_shuffle_playing_message_exvs2ob_message_setting_MessageSettingId" FOREIGN KEY ("MessageSettingId") REFERENCES "exvs2ob_message_setting" ("MessageSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_online_shuffle_result_message" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_online_shuffle_result_message" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "MessageSettingId" INTEGER NOT NULL,
                "TopMessageText" TEXT NOT NULL,
                "TopUniqueMessageId" INTEGER NOT NULL,
                "DownMessageText" TEXT NOT NULL,
                "DownUniqueMessageId" INTEGER NOT NULL,
                "LeftMessageText" TEXT NOT NULL,
                "LeftUniqueMessageId" INTEGER NOT NULL,
                "RightMessageText" TEXT NOT NULL,
                "RightUniqueMessageId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_online_shuffle_result_message_exvs2ob_message_setting_MessageSettingId" FOREIGN KEY ("MessageSettingId") REFERENCES "exvs2ob_message_setting" ("MessageSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_opening_message" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_opening_message" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "MessageSettingId" INTEGER NOT NULL,
                "TopMessageText" TEXT NOT NULL,
                "TopUniqueMessageId" INTEGER NOT NULL,
                "DownMessageText" TEXT NOT NULL,
                "DownUniqueMessageId" INTEGER NOT NULL,
                "LeftMessageText" TEXT NOT NULL,
                "LeftUniqueMessageId" INTEGER NOT NULL,
                "RightMessageText" TEXT NOT NULL,
                "RightUniqueMessageId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_opening_message_exvs2ob_message_setting_MessageSettingId" FOREIGN KEY ("MessageSettingId") REFERENCES "exvs2ob_message_setting" ("MessageSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_player_battle_statistic" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_player_battle_statistic" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "TotalGivenDamage" INTEGER NOT NULL,
                "TotalEnemyDefeatedCount" INTEGER NOT NULL,
                "TotalClassMatchTenConsecutiveWinCount" INTEGER NOT NULL,
                "TotalNoDamageBattleCount" INTEGER NOT NULL,
                "TotalExBurstDamage" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_player_battle_statistic_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_player_burst_statistic" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_player_burst_statistic" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "BurstType" INTEGER NOT NULL,
                "TotalBattle" INTEGER NOT NULL,
                "TotalWin" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_player_burst_statistic_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_player_profile" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_player_profile" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "OpenRecord" INTEGER NOT NULL,
                "OpenEchelon" INTEGER NOT NULL,
                "OpenSkillpoint" INTEGER NOT NULL,
                "FullLoadCardCount" INTEGER NOT NULL,
                "ExTutorialDispFlag" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_player_profile_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_playing_message" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_playing_message" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "MessageSettingId" INTEGER NOT NULL,
                "TopMessageText" TEXT NOT NULL,
                "TopUniqueMessageId" INTEGER NOT NULL,
                "DownMessageText" TEXT NOT NULL,
                "DownUniqueMessageId" INTEGER NOT NULL,
                "LeftMessageText" TEXT NOT NULL,
                "LeftUniqueMessageId" INTEGER NOT NULL,
                "RightMessageText" TEXT NOT NULL,
                "RightUniqueMessageId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_playing_message_exvs2ob_message_setting_MessageSettingId" FOREIGN KEY ("MessageSettingId") REFERENCES "exvs2ob_message_setting" ("MessageSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_pre_battle_history" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_pre_battle_history" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "CurrentConsecutiveWins" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_pre_battle_history_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_private_match_room" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_private_match_room" PRIMARY KEY AUTOINCREMENT,
                "PrivateMatchRoomSettingId" INTEGER NOT NULL,
                "TagName" TEXT NOT NULL,
                "TagType" INTEGER NOT NULL,
                "MatchingType" INTEGER NOT NULL,
                "MatchingAttribute" INTEGER NOT NULL,
                "RuleType" INTEGER NOT NULL,
                "SelectableMsIds" TEXT NOT NULL,
                "RevengeFlag" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_private_match_room_exvs2ob_user_private_match_room_setting_PrivateMatchRoomSettingId" FOREIGN KEY ("PrivateMatchRoomSettingId") REFERENCES "exvs2ob_user_private_match_room_setting" ("PrivateMatchRoomSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_quick_start_setting" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_quick_start_setting" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "QuickStartFlag" INTEGER NOT NULL,
                "GameMode" INTEGER NOT NULL,
                "RuleType" INTEGER NULL,
                "TeamType" INTEGER NOT NULL,
                "MstMobileSuitId0" INTEGER NOT NULL,
                "BurstType0" INTEGER NOT NULL,
                "MstMobileSuitId1" INTEGER NOT NULL,
                "BurstType1" INTEGER NOT NULL,
                "StageId" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_quick_start_setting_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_result_message" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_result_message" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "MessageSettingId" INTEGER NOT NULL,
                "TopMessageText" TEXT NOT NULL,
                "TopUniqueMessageId" INTEGER NOT NULL,
                "DownMessageText" TEXT NOT NULL,
                "DownUniqueMessageId" INTEGER NOT NULL,
                "LeftMessageText" TEXT NOT NULL,
                "LeftUniqueMessageId" INTEGER NOT NULL,
                "RightMessageText" TEXT NOT NULL,
                "RightUniqueMessageId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_result_message_exvs2ob_message_setting_MessageSettingId" FOREIGN KEY ("MessageSettingId") REFERENCES "exvs2ob_message_setting" ("MessageSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_shared_upload_replay" (
                "ReplayId" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_shared_upload_replay" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "Filename" TEXT NOT NULL,
                "ReplaySize" INTEGER NOT NULL,
                "PlayedAt" INTEGER NOT NULL,
                "StageId" INTEGER NOT NULL,
                "PilotsJson" TEXT NOT NULL,
                "SpecialFlag" INTEGER NOT NULL,
                "ReplayServiceFlag" INTEGER NOT NULL,
                "MobileUserId" INTEGER NOT NULL,
                "MatchingMode" INTEGER NOT NULL,
                "TeamType" INTEGER NOT NULL,
                "ReturnMatchFlag" INTEGER NOT NULL,
                "TournamentId" INTEGER NULL,
                "BattleClass" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_shared_upload_replay_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_stage_save_data" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_stage_save_data" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "PatternId" INTEGER NOT NULL,
                "Difficulty" INTEGER NOT NULL,
                "StageNum" INTEGER NOT NULL,
                "BossId1" INTEGER NOT NULL,
                "BossId2" INTEGER NULL,
                "MaxGauge" INTEGER NOT NULL,
                "CurrentGauge" INTEGER NOT NULL,
                "MstMobileSuitId" INTEGER NOT NULL,
                "CustomChipIds" TEXT NOT NULL,
                "SaveDataUrl" TEXT NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_stage_save_data_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_tag_team_data" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_tag_team_data" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "TeamName" TEXT NOT NULL,
                "TeammateCardId" INTEGER NOT NULL,
                "OnlineRankPoint" INTEGER NOT NULL,
                "BackgroundPartsId" INTEGER NOT NULL,
                "EffectId" INTEGER NOT NULL,
                "EmblemId" INTEGER NOT NULL,
                "SkillPoint" INTEGER NOT NULL,
                "SkillPointBoost" INTEGER NOT NULL,
                "BgmId" INTEGER NOT NULL,
                "NameColorId" INTEGER NOT NULL,
                "BoostRemains" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_tag_team_data_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_team_setting" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_team_setting" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "QuickOnlineTagCardId" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_team_setting_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_training_profile" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_training_profile" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "MstMobileSuitId" INTEGER NOT NULL,
                "BurstType" INTEGER NOT NULL,
                "CpuLevel" INTEGER NOT NULL,
                "ExBurstGauge" INTEGER NOT NULL,
                "DamageDisplay" INTEGER NOT NULL,
                "CpuAutoGuard" INTEGER NOT NULL,
                "CommandGuideDisplay" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL, "Armor" INTEGER NOT NULL DEFAULT 0, "CpuAutoHeal" INTEGER NOT NULL DEFAULT 0, "ExOverLimit" INTEGER NOT NULL DEFAULT 0, "FastReload" INTEGER NOT NULL DEFAULT 0,
                CONSTRAINT "FK_exvs2ob_training_profile_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_triad_clear_time" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_triad_clear_time" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "Year" INTEGER NOT NULL,
                "Month" INTEGER NOT NULL,
                "CourseClearTime" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_triad_clear_time_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_triad_course_data" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_triad_course_data" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "CourseId" INTEGER NOT NULL,
                "ReleasedAt" INTEGER NOT NULL,
                "TotalPlayNum" INTEGER NOT NULL,
                "TotalClearNum" INTEGER NOT NULL,
                "Highscore" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_triad_course_data_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_triad_high_score" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_triad_high_score" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "Year" INTEGER NOT NULL,
                "Month" INTEGER NOT NULL,
                "CourseHighScore" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_triad_high_score_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_triad_misc_info" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_triad_misc_info" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "CpuRibbons" TEXT NOT NULL,
                "TotalTriadScore" INTEGER NOT NULL,
                "TotalTriadWantedDefeatNum" INTEGER NOT NULL,
                "TotalTriadScenePlayNum" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_triad_misc_info_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_triad_partner" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_triad_partner" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "MstMobileSuitId" INTEGER NOT NULL,
                "ArmorLevel" INTEGER NOT NULL,
                "ShootAttackLevel" INTEGER NOT NULL,
                "InfightAttackLevel" INTEGER NOT NULL,
                "BoosterLevel" INTEGER NOT NULL,
                "ExGaugeLevel" INTEGER NOT NULL,
                "AiLevel" INTEGER NOT NULL,
                "BurstType" INTEGER NOT NULL,
                "MsSkill1" INTEGER NOT NULL,
                "MsSkill2" INTEGER NOT NULL,
                "TriadTeamName" TEXT NOT NULL,
                "TriadBackgroundPartsId" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_triad_partner_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_triad_target_defeated_count" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_triad_target_defeated_count" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "Year" INTEGER NOT NULL,
                "Month" INTEGER NOT NULL,
                "DestroyCount" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_triad_target_defeated_count_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_triad_wanted_defeated_count" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_triad_wanted_defeated_count" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "Year" INTEGER NOT NULL,
                "Month" INTEGER NOT NULL,
                "DestroyCount" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_triad_wanted_defeated_count_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_upload_image" (
                "ImageId" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_upload_image" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "Filename" TEXT NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_upload_image_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_upload_replay" (
                "ReplayId" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_upload_replay" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "Filename" TEXT NOT NULL,
                "ReplaySize" INTEGER NOT NULL,
                "PlayedAt" INTEGER NOT NULL,
                "StageId" INTEGER NOT NULL,
                "PilotsJson" TEXT NOT NULL,
                "SpecialFlag" INTEGER NOT NULL,
                "ReplayServiceFlag" INTEGER NOT NULL,
                "MobileUserId" INTEGER NOT NULL,
                "MatchingMode" INTEGER NOT NULL,
                "TeamType" INTEGER NOT NULL,
                "ReturnMatchFlag" INTEGER NOT NULL,
                "TournamentId" INTEGER NULL,
                "BattleClass" INTEGER NOT NULL,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                CONSTRAINT "FK_exvs2ob_upload_replay_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id")
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_user_class_match_title" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_user_class_match_title" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "TitleTextId" INTEGER NOT NULL,
                "TitleOrnamentId" INTEGER NOT NULL,
                "TitleEffectId" INTEGER NOT NULL,
                "TitleBackgroundPartsId" INTEGER NOT NULL,
                "CustomText" TEXT NOT NULL,
                "UserTitleSettingId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_user_class_match_title_exvs2ob_user_title_setting_UserTitleSettingId" FOREIGN KEY ("UserTitleSettingId") REFERENCES "exvs2ob_user_title_setting" ("UserTitleSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_user_default_title" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_user_default_title" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "TitleTextId" INTEGER NOT NULL,
                "TitleOrnamentId" INTEGER NOT NULL,
                "TitleEffectId" INTEGER NOT NULL,
                "TitleBackgroundPartsId" INTEGER NOT NULL,
                "CustomText" TEXT NOT NULL,
                "UserTitleSettingId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_user_default_title_exvs2ob_user_title_setting_UserTitleSettingId" FOREIGN KEY ("UserTitleSettingId") REFERENCES "exvs2ob_user_title_setting" ("UserTitleSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_user_private_match_room_setting" (
                "PrivateMatchRoomSettingId" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_user_private_match_room_setting" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "EnablePrivateMatch" INTEGER NOT NULL,
                "IsPrivateMatchHost" INTEGER NOT NULL,
                "ParticipatedPrivateRoomId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_user_private_match_room_setting_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_user_title_setting" (
                "UserTitleSettingId" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_user_title_setting" PRIMARY KEY AUTOINCREMENT,
                "CardId" INTEGER NOT NULL,
                "RandomTitleFlag" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_user_title_setting_exvs2ob_card_profile_CardId" FOREIGN KEY ("CardId") REFERENCES "exvs2ob_card_profile" ("Id") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE TABLE "exvs2ob_user_triad_title" (
                "Id" INTEGER NOT NULL CONSTRAINT "PK_exvs2ob_user_triad_title" PRIMARY KEY AUTOINCREMENT,
                "CreateTime" TEXT NOT NULL,
                "UpdateTime" TEXT NOT NULL,
                "TitleTextId" INTEGER NOT NULL,
                "TitleOrnamentId" INTEGER NOT NULL,
                "TitleEffectId" INTEGER NOT NULL,
                "TitleBackgroundPartsId" INTEGER NOT NULL,
                "CustomText" TEXT NOT NULL,
                "UserTitleSettingId" INTEGER NOT NULL,
                CONSTRAINT "FK_exvs2ob_user_triad_title_exvs2ob_user_title_setting_UserTitleSettingId" FOREIGN KEY ("UserTitleSettingId") REFERENCES "exvs2ob_user_title_setting" ("UserTitleSettingId") ON DELETE CASCADE
            );
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_action_log_BattleHistoryId_CardId" ON "exvs2ob_battle_action_log" ("BattleHistoryId", "CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_action_log_Id" ON "exvs2ob_battle_action_log" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_action_log_Id_BattleHistoryId" ON "exvs2ob_battle_action_log" ("Id", "BattleHistoryId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_battle_ally_BattleHistoryId" ON "exvs2ob_battle_ally" ("BattleHistoryId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_ally_BattleHistoryId_CardId" ON "exvs2ob_battle_ally" ("BattleHistoryId", "CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_ally_Id" ON "exvs2ob_battle_ally" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_ally_Id_BattleHistoryId" ON "exvs2ob_battle_ally" ("Id", "BattleHistoryId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_history_CardId" ON "exvs2ob_battle_history" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_history_Id" ON "exvs2ob_battle_history" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_history_Id_CardId" ON "exvs2ob_battle_history" ("Id", "CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_history_PlayedAt" ON "exvs2ob_battle_history" ("PlayedAt");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_battle_player_level_CardId" ON "exvs2ob_battle_player_level" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_player_level_Id" ON "exvs2ob_battle_player_level" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_battle_self_BattleHistoryId" ON "exvs2ob_battle_self" ("BattleHistoryId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_self_BattleHistoryId_CardId" ON "exvs2ob_battle_self" ("BattleHistoryId", "CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_self_Id" ON "exvs2ob_battle_self" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_self_Id_BattleHistoryId" ON "exvs2ob_battle_self" ("Id", "BattleHistoryId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_battle_solo_class_record_CardId" ON "exvs2ob_battle_solo_class_record" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_solo_class_record_Id" ON "exvs2ob_battle_solo_class_record" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_target_BattleHistoryId_CardId" ON "exvs2ob_battle_target" ("BattleHistoryId", "CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_target_Id" ON "exvs2ob_battle_target" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_target_Id_BattleHistoryId" ON "exvs2ob_battle_target" ("Id", "BattleHistoryId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_battle_team_class_record_CardId" ON "exvs2ob_battle_team_class_record" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_team_class_record_Id" ON "exvs2ob_battle_team_class_record" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_battle_win_loss_record_CardId" ON "exvs2ob_battle_win_loss_record" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_battle_win_loss_record_Id" ON "exvs2ob_battle_win_loss_record" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_boost_setting_CardId" ON "exvs2ob_boost_setting" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_boost_setting_Id" ON "exvs2ob_boost_setting" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_card_profile_Id" ON "exvs2ob_card_profile" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_challenge_mission_profile_CardId" ON "exvs2ob_challenge_mission_profile" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_challenge_mission_profile_CardId_EffectiveYear_EffectiveMonth_EffectiveDay" ON "exvs2ob_challenge_mission_profile" ("CardId", "EffectiveYear", "EffectiveMonth", "EffectiveDay");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_challenge_mission_profile_Id" ON "exvs2ob_challenge_mission_profile" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_command_message_data_Id" ON "exvs2ob_command_message_data" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_command_message_data_MessageSettingId" ON "exvs2ob_command_message_data" ("MessageSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_command_message_data_MessageSettingId_Mode" ON "exvs2ob_command_message_data" ("MessageSettingId", "Mode");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_customize_profile_CardId" ON "exvs2ob_customize_profile" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_customize_profile_Id" ON "exvs2ob_customize_profile" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_default_sticker_profile_CardId" ON "exvs2ob_default_sticker_profile" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_default_sticker_profile_Id" ON "exvs2ob_default_sticker_profile" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_favourite_ms_CardId" ON "exvs2ob_favourite_ms" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_favourite_ms_CardId_MstMobileSuitId" ON "exvs2ob_favourite_ms" ("CardId", "MstMobileSuitId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_favourite_ms_Id" ON "exvs2ob_favourite_ms" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_gamepad_setting_CardId" ON "exvs2ob_gamepad_setting" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_gamepad_setting_Id" ON "exvs2ob_gamepad_setting" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_general_setting_CardId" ON "exvs2ob_general_setting" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_general_setting_Id" ON "exvs2ob_general_setting" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_invalid_visit_Id" ON "exvs2ob_invalid_visit" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_license_score_record_CardId" ON "exvs2ob_license_score_record" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_license_score_record_Id" ON "exvs2ob_license_score_record" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_message_setting_CardId" ON "exvs2ob_message_setting" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_message_setting_MessageSettingId" ON "exvs2ob_message_setting" ("MessageSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_ms_class_match_title_FavouriteMobileSuitId" ON "exvs2ob_ms_class_match_title" ("FavouriteMobileSuitId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_class_match_title_Id" ON "exvs2ob_ms_class_match_title" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_ms_default_title_FavouriteMobileSuitId" ON "exvs2ob_ms_default_title" ("FavouriteMobileSuitId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_default_title_Id" ON "exvs2ob_ms_default_title" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_pvp_stat_CardId" ON "exvs2ob_ms_pvp_stat" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_pvp_stat_CardId_MstMobileSuitId" ON "exvs2ob_ms_pvp_stat" ("CardId", "MstMobileSuitId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_pvp_stat_Id" ON "exvs2ob_ms_pvp_stat" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_sticker_CardId" ON "exvs2ob_ms_sticker" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_sticker_CardId_MstMobileSuitId" ON "exvs2ob_ms_sticker" ("CardId", "MstMobileSuitId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_sticker_StickerId" ON "exvs2ob_ms_sticker" ("StickerId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_ms_triad_title_FavouriteMobileSuitId" ON "exvs2ob_ms_triad_title" ("FavouriteMobileSuitId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_triad_title_Id" ON "exvs2ob_ms_triad_title" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_usage_CardId" ON "exvs2ob_ms_usage" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_usage_CardId_MstMobileSuitId" ON "exvs2ob_ms_usage" ("CardId", "MstMobileSuitId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_ms_usage_Id" ON "exvs2ob_ms_usage" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_navi_CardId" ON "exvs2ob_navi" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_navi_CardId_GuestNavId" ON "exvs2ob_navi" ("CardId", "GuestNavId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_navi_Id" ON "exvs2ob_navi" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_navi_setting_CardId" ON "exvs2ob_navi_setting" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_navi_setting_Id" ON "exvs2ob_navi_setting" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_online_pair_CardId" ON "exvs2ob_online_pair" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_online_shuffle_opening_message_Id" ON "exvs2ob_online_shuffle_opening_message" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_online_shuffle_opening_message_MessageSettingId" ON "exvs2ob_online_shuffle_opening_message" ("MessageSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_online_shuffle_playing_message_Id" ON "exvs2ob_online_shuffle_playing_message" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_online_shuffle_playing_message_MessageSettingId" ON "exvs2ob_online_shuffle_playing_message" ("MessageSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_online_shuffle_result_message_Id" ON "exvs2ob_online_shuffle_result_message" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_online_shuffle_result_message_MessageSettingId" ON "exvs2ob_online_shuffle_result_message" ("MessageSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_opening_message_Id" ON "exvs2ob_opening_message" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_opening_message_MessageSettingId" ON "exvs2ob_opening_message" ("MessageSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_player_battle_statistic_CardId" ON "exvs2ob_player_battle_statistic" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_player_battle_statistic_Id" ON "exvs2ob_player_battle_statistic" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_player_burst_statistic_CardId" ON "exvs2ob_player_burst_statistic" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_player_burst_statistic_CardId_BurstType" ON "exvs2ob_player_burst_statistic" ("CardId", "BurstType");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_player_burst_statistic_Id" ON "exvs2ob_player_burst_statistic" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_player_profile_CardId" ON "exvs2ob_player_profile" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_player_profile_Id" ON "exvs2ob_player_profile" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_playing_message_Id" ON "exvs2ob_playing_message" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_playing_message_MessageSettingId" ON "exvs2ob_playing_message" ("MessageSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_pre_battle_history_CardId" ON "exvs2ob_pre_battle_history" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_pre_battle_history_Id" ON "exvs2ob_pre_battle_history" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_private_match_room_Id" ON "exvs2ob_private_match_room" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_private_match_room_PrivateMatchRoomSettingId" ON "exvs2ob_private_match_room" ("PrivateMatchRoomSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_quick_start_setting_CardId" ON "exvs2ob_quick_start_setting" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_quick_start_setting_Id" ON "exvs2ob_quick_start_setting" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_result_message_Id" ON "exvs2ob_result_message" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_result_message_MessageSettingId" ON "exvs2ob_result_message" ("MessageSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_shared_upload_replay_CardId_ReplayId" ON "exvs2ob_shared_upload_replay" ("CardId", "ReplayId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_shared_upload_replay_ReplayId" ON "exvs2ob_shared_upload_replay" ("ReplayId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_stage_save_data_CardId" ON "exvs2ob_stage_save_data" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_stage_save_data_Id" ON "exvs2ob_stage_save_data" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_tag_team_data_CardId" ON "exvs2ob_tag_team_data" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_team_setting_CardId" ON "exvs2ob_team_setting" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_team_setting_Id" ON "exvs2ob_team_setting" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_training_profile_CardId" ON "exvs2ob_training_profile" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_training_profile_Id" ON "exvs2ob_training_profile" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_triad_clear_time_CardId_Year_Month" ON "exvs2ob_triad_clear_time" ("CardId", "Year", "Month");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_triad_clear_time_Id" ON "exvs2ob_triad_clear_time" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_triad_course_data_CardId_CourseId" ON "exvs2ob_triad_course_data" ("CardId", "CourseId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_triad_course_data_Id" ON "exvs2ob_triad_course_data" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_triad_high_score_CardId_Year_Month" ON "exvs2ob_triad_high_score" ("CardId", "Year", "Month");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_triad_high_score_Id" ON "exvs2ob_triad_high_score" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_triad_misc_info_CardId" ON "exvs2ob_triad_misc_info" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_triad_misc_info_Id" ON "exvs2ob_triad_misc_info" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_triad_partner_CardId" ON "exvs2ob_triad_partner" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_triad_partner_Id" ON "exvs2ob_triad_partner" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_triad_target_defeated_count_CardId_Year_Month" ON "exvs2ob_triad_target_defeated_count" ("CardId", "Year", "Month");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_triad_target_defeated_count_Id" ON "exvs2ob_triad_target_defeated_count" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_triad_wanted_defeated_count_CardId_Year_Month" ON "exvs2ob_triad_wanted_defeated_count" ("CardId", "Year", "Month");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_triad_wanted_defeated_count_Id" ON "exvs2ob_triad_wanted_defeated_count" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_upload_image_CardId_ImageId" ON "exvs2ob_upload_image" ("CardId", "ImageId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_upload_image_ImageId" ON "exvs2ob_upload_image" ("ImageId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_upload_replay_CardId_ReplayId" ON "exvs2ob_upload_replay" ("CardId", "ReplayId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_upload_replay_ReplayId" ON "exvs2ob_upload_replay" ("ReplayId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_user_class_match_title_Id" ON "exvs2ob_user_class_match_title" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_user_class_match_title_UserTitleSettingId" ON "exvs2ob_user_class_match_title" ("UserTitleSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_user_default_title_Id" ON "exvs2ob_user_default_title" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_user_default_title_UserTitleSettingId" ON "exvs2ob_user_default_title" ("UserTitleSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_user_private_match_room_setting_CardId" ON "exvs2ob_user_private_match_room_setting" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_user_private_match_room_setting_PrivateMatchRoomSettingId" ON "exvs2ob_user_private_match_room_setting" ("PrivateMatchRoomSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_user_title_setting_CardId" ON "exvs2ob_user_title_setting" ("CardId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_user_title_setting_UserTitleSettingId" ON "exvs2ob_user_title_setting" ("UserTitleSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE INDEX "IX_exvs2ob_user_triad_title_Id" ON "exvs2ob_user_triad_title" ("Id");
            """);

        migrationBuilder.Sql("""
            CREATE UNIQUE INDEX "IX_exvs2ob_user_triad_title_UserTitleSettingId" ON "exvs2ob_user_triad_title" ("UserTitleSettingId");
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_burst_usage_view as
            	            select BurstType, SUM(TotalBattle) as AggregatedTotalBattle, SUM(TotalWin) as AggregatedTotalWin
            	            from exvs2ob_player_burst_statistic
            	            group by BurstType
            	            order by BurstType;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_mobile_suit_usage_view as
            	            select MstMobileSuitId, SUM(TotalBattleCount) as AggregatedTotalBattle, SUM(TotalWinCount) as AggregatedTotalWin
            	            from exvs2ob_ms_pvp_stat
            	            group by MstMobileSuitId
            	            order by MstMobileSuitId;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_player_level_rank_view as
            	            select DENSE_RANK() OVER (ORDER BY PrestigeId desc, PlayerLevelId desc, PlayerExp desc)
            	            as Rank, CardProfile.UserName, 
            	                     PlayerLevel.PrestigeId, PlayerLevel.PlayerLevelId, PlayerLevel.PlayerExp, 
            	                     TotalWin, TotalLose
            	            from exvs2ob_battle_player_level PlayerLevel
            	            join exvs2ob_card_profile CardProfile
            	            on PlayerLevel.CardId = CardProfile.Id
            	            join exvs2ob_battle_win_loss_record WinLossRecord
            	            on PlayerLevel.CardId = WinLossRecord.CardId;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_solo_ace_percentile_view as
                            WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                                       FROM exvs2ob_battle_solo_class_record
                                       WHERE ClassId = 3)
                            SELECT RatePercentile, MAX(rate) as RatePoint
                            FROM p
                            GROUP BY RatePercentile;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_solo_over_percentile_view as
                            WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                                       FROM exvs2ob_battle_solo_class_record
                                       WHERE ClassId = 4)
                            SELECT RatePercentile, MAX(rate) as RatePoint
                            FROM p
                            GROUP BY RatePercentile;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_solo_over_rank_view as
            				select DENSE_RANK() OVER (ORDER BY ClassId DESC, Rate DESC)
            				as Rank, CardId, Rate
            				from exvs2ob_battle_solo_class_record where ClassId = 4;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_solo_pilot_percentile_view as
                            WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                                       FROM exvs2ob_battle_solo_class_record
                                       WHERE ClassId = 1)
                            SELECT RatePercentile, MAX(rate) as RatePoint
                            FROM p
                            GROUP BY RatePercentile;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_solo_valiant_percentile_view as
                            WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                                       FROM exvs2ob_battle_solo_class_record
                                       WHERE ClassId = 2)
                            SELECT RatePercentile, MAX(rate) as RatePoint
                            FROM p
                            GROUP BY RatePercentile;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_team_ace_percentile_view as
                            WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                                       FROM exvs2ob_battle_team_class_record
                                       WHERE ClassId = 3)
                            SELECT RatePercentile, MAX(rate) as RatePoint
                            FROM p
                            GROUP BY RatePercentile;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_team_over_percentile_view as
            				WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
            						   FROM exvs2ob_battle_team_class_record
            						   WHERE ClassId = 4)
            				SELECT RatePercentile, MAX(rate) as RatePoint
            				FROM p
            				GROUP BY RatePercentile;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_team_over_rank_view as
            				select DENSE_RANK() OVER (ORDER BY ClassId DESC, Rate DESC)
            				as Rank, CardId, Rate
            				from exvs2ob_battle_team_class_record where ClassId = 4;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_team_pilot_percentile_view as
                            WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                                       FROM exvs2ob_battle_team_class_record
                                       WHERE ClassId = 1)
                            SELECT RatePercentile, MAX(rate) as RatePoint
                            FROM p
                            GROUP BY RatePercentile;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_team_valiant_percentile_view as
                            WITH p AS (SELECT rate, NTILE(100) OVER (ORDER BY rate desc) AS RatePercentile
                                       FROM exvs2ob_battle_team_class_record
                                       WHERE ClassId = 2)
                            SELECT RatePercentile, MAX(rate) as RatePoint
                            FROM p
                            GROUP BY RatePercentile;
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_triad_clear_time_view as
                            select DENSE_RANK() OVER (ORDER BY CourseClearTime)
                            as Rank, CardId, CourseClearTime, Year, Month
                            from exvs2ob_triad_clear_time 
                            where Year = strftime('%Y', CURRENT_TIMESTAMP)
                            and Month = ltrim(strftime('%m', CURRENT_TIMESTAMP), '0');
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_triad_high_score_view as
                            select DENSE_RANK() OVER (ORDER BY CourseHighScore desc)
                            as Rank, CardId, CourseHighScore, Year, Month
                            from exvs2ob_triad_high_score
                            where Year = strftime('%Y', CURRENT_TIMESTAMP)
                            and Month = ltrim(strftime('%m', CURRENT_TIMESTAMP), '0');
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_triad_target_defeated_count_view as
                            select DENSE_RANK() OVER (ORDER BY DestroyCount desc)
                                       as Rank, CardId, DestroyCount, Year, Month
                            from exvs2ob_triad_target_defeated_count
                            where Year = strftime('%Y', CURRENT_TIMESTAMP)
                            and Month = ltrim(strftime('%m', CURRENT_TIMESTAMP), '0');
            """);

        migrationBuilder.Sql("""
            CREATE VIEW exvs2ob_triad_wanted_defeated_count_view as
                            select DENSE_RANK() OVER (ORDER BY DestroyCount desc)
                            as Rank, CardId, DestroyCount, Year, Month
                            from exvs2ob_triad_wanted_defeated_count
                            where Year = strftime('%Y', CURRENT_TIMESTAMP)
                            and Month = ltrim(strftime('%m', CURRENT_TIMESTAMP), '0');
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_burst_usage_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_mobile_suit_usage_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_player_level_rank_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_solo_ace_percentile_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_solo_over_percentile_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_solo_over_rank_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_solo_pilot_percentile_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_solo_valiant_percentile_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_team_ace_percentile_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_team_over_percentile_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_team_over_rank_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_team_pilot_percentile_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_team_valiant_percentile_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_triad_clear_time_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_triad_high_score_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_triad_target_defeated_count_view\";");
        migrationBuilder.Sql("DROP VIEW \"exvs2ob_triad_wanted_defeated_count_view\";");
        migrationBuilder.DropTable(name: "exvs2ob_battle_action_log");
        migrationBuilder.DropTable(name: "exvs2ob_battle_ally");
        migrationBuilder.DropTable(name: "exvs2ob_battle_player_level");
        migrationBuilder.DropTable(name: "exvs2ob_battle_self");
        migrationBuilder.DropTable(name: "exvs2ob_battle_solo_class_record");
        migrationBuilder.DropTable(name: "exvs2ob_battle_target");
        migrationBuilder.DropTable(name: "exvs2ob_battle_team_class_record");
        migrationBuilder.DropTable(name: "exvs2ob_battle_win_loss_record");
        migrationBuilder.DropTable(name: "exvs2ob_boost_setting");
        migrationBuilder.DropTable(name: "exvs2ob_challenge_mission_profile");
        migrationBuilder.DropTable(name: "exvs2ob_command_message_data");
        migrationBuilder.DropTable(name: "exvs2ob_customize_profile");
        migrationBuilder.DropTable(name: "exvs2ob_default_sticker_profile");
        migrationBuilder.DropTable(name: "exvs2ob_gamepad_setting");
        migrationBuilder.DropTable(name: "exvs2ob_general_setting");
        migrationBuilder.DropTable(name: "exvs2ob_invalid_visit");
        migrationBuilder.DropTable(name: "exvs2ob_license_score_record");
        migrationBuilder.DropTable(name: "exvs2ob_ms_class_match_title");
        migrationBuilder.DropTable(name: "exvs2ob_ms_default_title");
        migrationBuilder.DropTable(name: "exvs2ob_ms_pvp_stat");
        migrationBuilder.DropTable(name: "exvs2ob_ms_sticker");
        migrationBuilder.DropTable(name: "exvs2ob_ms_triad_title");
        migrationBuilder.DropTable(name: "exvs2ob_ms_usage");
        migrationBuilder.DropTable(name: "exvs2ob_navi");
        migrationBuilder.DropTable(name: "exvs2ob_navi_setting");
        migrationBuilder.DropTable(name: "exvs2ob_online_pair");
        migrationBuilder.DropTable(name: "exvs2ob_online_shuffle_opening_message");
        migrationBuilder.DropTable(name: "exvs2ob_online_shuffle_playing_message");
        migrationBuilder.DropTable(name: "exvs2ob_online_shuffle_result_message");
        migrationBuilder.DropTable(name: "exvs2ob_opening_message");
        migrationBuilder.DropTable(name: "exvs2ob_player_battle_statistic");
        migrationBuilder.DropTable(name: "exvs2ob_player_burst_statistic");
        migrationBuilder.DropTable(name: "exvs2ob_player_profile");
        migrationBuilder.DropTable(name: "exvs2ob_playing_message");
        migrationBuilder.DropTable(name: "exvs2ob_pre_battle_history");
        migrationBuilder.DropTable(name: "exvs2ob_private_match_room");
        migrationBuilder.DropTable(name: "exvs2ob_quick_start_setting");
        migrationBuilder.DropTable(name: "exvs2ob_result_message");
        migrationBuilder.DropTable(name: "exvs2ob_shared_upload_replay");
        migrationBuilder.DropTable(name: "exvs2ob_stage_save_data");
        migrationBuilder.DropTable(name: "exvs2ob_tag_team_data");
        migrationBuilder.DropTable(name: "exvs2ob_team_setting");
        migrationBuilder.DropTable(name: "exvs2ob_training_profile");
        migrationBuilder.DropTable(name: "exvs2ob_triad_clear_time");
        migrationBuilder.DropTable(name: "exvs2ob_triad_course_data");
        migrationBuilder.DropTable(name: "exvs2ob_triad_high_score");
        migrationBuilder.DropTable(name: "exvs2ob_triad_misc_info");
        migrationBuilder.DropTable(name: "exvs2ob_triad_partner");
        migrationBuilder.DropTable(name: "exvs2ob_triad_target_defeated_count");
        migrationBuilder.DropTable(name: "exvs2ob_triad_wanted_defeated_count");
        migrationBuilder.DropTable(name: "exvs2ob_upload_image");
        migrationBuilder.DropTable(name: "exvs2ob_upload_replay");
        migrationBuilder.DropTable(name: "exvs2ob_user_class_match_title");
        migrationBuilder.DropTable(name: "exvs2ob_user_default_title");
        migrationBuilder.DropTable(name: "exvs2ob_user_triad_title");
        migrationBuilder.DropTable(name: "exvs2ob_battle_history");
        migrationBuilder.DropTable(name: "exvs2ob_favourite_ms");
        migrationBuilder.DropTable(name: "exvs2ob_message_setting");
        migrationBuilder.DropTable(name: "exvs2ob_user_private_match_room_setting");
        migrationBuilder.DropTable(name: "exvs2ob_user_title_setting");
        migrationBuilder.DropTable(name: "exvs2ob_card_profile");
    }
}
