namespace ServerInfinite.Models.Config;

/// <summary>
/// Values sent in Response.LoadGameData fields 21-36 / 38 (vsac29 protocol).
/// The game client applies the badge (route battle level) rules itself; the server only
/// distributes these parameters. Defaults are placeholders and can be overridden in
/// Configurations/server.json -> CardServerConfig.GameConfigurations.RouteBattleConfigurations.
/// </summary>
public class RouteBattleConfigurations
{
    public uint VsPlayerLevelPoint { get; set; } = 10u;

    public uint CpuPlayerLevelPoint { get; set; } = 5u;

    public uint ClassWinBadgePoint { get; set; } = 20u;

    public int ClassLoseBadgePoint { get; set; } = -10;

    public uint ClassWinBadgeBonus1 { get; set; } = 5u;

    public int ClassWinBadgeBonus2 { get; set; } = 10;

    public uint ClassLoseBadgeBonus { get; set; } = 5u;

    public uint RootWinBadgePoint1 { get; set; } = 10u;

    public uint RootWinBadgePoint2 { get; set; } = 20u;

    public uint RootWinBadgePoint3 { get; set; } = 30u;

    public uint RootWinBadgePoint4 { get; set; } = 40u;

    public int RootLoseBadgePoint { get; set; } = -5;

    public uint RootDifficultyBadgeBonus1 { get; set; } = 0u;

    public uint RootDifficultyBadgeBonus2 { get; set; } = 5u;

    public uint RootDifficultyBadgeBonus3 { get; set; } = 10u;

    public uint RootPatternId { get; set; } = 1u;

    public bool GpuHealthCheckReportAvailableFlag { get; set; }

    // ===== Badge persistence =====
    // The game decides badge up/down and reports the resulting badge_id in
    // SaveVsmOnResult.result.badge_id (19) and SaveUserPlayResearchData.badge_id (6).
    // obtained_badge_point (SaveVscResult 25 / SaveVsmOnResult 69) is accumulated into BadgeExp.

    /// <summary>When the reported badge_id differs from the stored one, reset BadgeExp
    /// (mirrors LoadGameData.BadgeExpTable up_default_exp / down_default_exp).</summary>
    public bool ResetBadgeExpOnChange { get; set; } = true;

    /// <summary>BadgeExp after a badge rank up (badge_id increased).</summary>
    public int BadgeUpDefaultExp { get; set; }

    /// <summary>BadgeExp after a badge rank down (badge_id decreased).</summary>
    public int BadgeDownDefaultExp { get; set; }

    /// <summary>Also take badge_id from SaveUserPlayResearchData (sent for every play, incl. route battle).</summary>
    public bool BadgeIdFromResearchData { get; set; } = true;

    /// <summary>Default save_data_url returned in LoadCard.save_data_info when none stored.</summary>
    public string SaveDataUrl { get; set; } = string.Empty;
}
