namespace ServerInfinite.Constants;

public static class TrackerTypes
{
    public const uint PilotStatisticType = 1;
    public const uint MobileSuitStatisticType = 2;
    public const uint PlayerLevelStatisticType = 4;
    public const uint SoloClassStatisticType = 5;
    public const uint TeamClassStatisticRate = 6;
    public const uint XbBattleType = 7;
    public const uint LicenseScore = 8;
    public const uint TitleType = 9;

    /// <summary>
    /// Resolves the tracker type purely from the tracker text_id's Category
    /// (mirrors WebUI TrackerTypes.json). Mapping:
    /// Pilot -> 1, MS -> 2, PlayerLevel -> 4, ClassMatchSolo -> 5,
    /// ClassMatchTeam -> 6, OB -> 7, Title -> 9.
    /// Unknown ids fall back to the Pilot type.
    /// </summary>
    public static uint ResolveTypeByTextId(uint textId) => textId switch
    {
        9 => PlayerLevelStatisticType,                 // PlayerLevel
        10 => SoloClassStatisticType,                  // ClassMatchSolo
        11 => TeamClassStatisticRate,                  // ClassMatchTeam
        20 => XbBattleType,                            // OB
        24 => TitleType,                               // Title
        25 => MobileSuitStatisticType,                 // MS (daily class match win rate)
        >= 12 and <= 19 => MobileSuitStatisticType,    // MS
        _ => PilotStatisticType,                       // Pilot (1-8, 21-23) and fallback
    };
}