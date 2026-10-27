using nue.protocol.exvs;
using ServerInfinite.Models.Config;

namespace ServerInfinite.Commands.LoadGameData;

/// <summary>
/// LoadGameData 21-36 / 38 (route battle / badge parameters, vsac29) and
/// XrossFesSetting 8/9 (playable_start_date / playable_end_date).
/// </summary>
public class RouteBattleSettingCommand : ILoadGameDataCommand
{
    private readonly CardServerConfig _config;

    public RouteBattleSettingCommand(CardServerConfig config)
    {
        _config = config;
    }

    public void Fill(Response.LoadGameData loadGameData)
    {
        var c = _config.GameConfigurations.RouteBattleConfigurations ?? new RouteBattleConfigurations();
        loadGameData.VsPlayerLevelPoint = c.VsPlayerLevelPoint;
        loadGameData.CpuPlayerLevelPoint = c.CpuPlayerLevelPoint;
        loadGameData.ClassWinBadgePoint = c.ClassWinBadgePoint;
        loadGameData.ClassLoseBadgePoint = c.ClassLoseBadgePoint;
        loadGameData.ClassWinBadgeBonus1 = c.ClassWinBadgeBonus1;
        loadGameData.ClassWinBadgeBonus2 = c.ClassWinBadgeBonus2;
        loadGameData.ClassLoseBadgeBonus = c.ClassLoseBadgeBonus;
        loadGameData.RootWinBadgePoint1 = c.RootWinBadgePoint1;
        loadGameData.RootWinBadgePoint2 = c.RootWinBadgePoint2;
        loadGameData.RootWinBadgePoint3 = c.RootWinBadgePoint3;
        loadGameData.RootWinBadgePoint4 = c.RootWinBadgePoint4;
        loadGameData.RootLoseBadgePoint = c.RootLoseBadgePoint;
        loadGameData.RootDifficultyBadgeBonus1 = c.RootDifficultyBadgeBonus1;
        loadGameData.RootDifficultyBadgeBonus2 = c.RootDifficultyBadgeBonus2;
        loadGameData.RootDifficultyBadgeBonus3 = c.RootDifficultyBadgeBonus3;
        loadGameData.RootPatternId = c.RootPatternId;
        loadGameData.GpuHealthCheckReportAvailableFlag = c.GpuHealthCheckReportAvailableFlag;

        if (loadGameData.FesSetting != null)
        {
            loadGameData.FesSetting.PlayableStartDate = loadGameData.FesSetting.StartDate;
            loadGameData.FesSetting.PlayableEndDate = loadGameData.FesSetting.EndDate;
        }
    }
}
