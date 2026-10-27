namespace WebUIInfinite.Shared.Dto.Statistics;

/// <summary>
/// 单个对战模式的战绩（场内/排位、单人/组队）。
/// </summary>
public class BattleStatisticsDto
{
    public uint BattleCount { get; set; } = 0;
    public uint WinCount { get; set; } = 0;
    public uint LoseCount { get; set; } = 0;
    public double WinRate { get; set; } = 0;
}

/// <summary>
/// 单台机体的战绩与熟练度。
/// </summary>
public class MobileSuitStatisticsDto
{
    public uint MstMobileSuitId { get; set; } = 0;
    public uint BattleCount { get; set; } = 0;
    public uint WinCount { get; set; } = 0;
    public uint LoseCount { get; set; } = 0;
    public double WinRate { get; set; } = 0;

    /// <summary>
    /// 机体使用次数，对应游戏内的熟练度点数。
    /// </summary>
    public uint MasteryPoint { get; set; } = 0;
}

/// <summary>
/// 卡片管理页（修改卡信息 / 显示玩家资料）底部展示的玩家战绩汇总。
/// </summary>
public class PlayerStatisticsDto
{
    /// <summary>店内单人对战</summary>
    public BattleStatisticsDto OfflineSolo { get; set; } = new();

    /// <summary>店内组队对战</summary>
    public BattleStatisticsDto OfflineTeam { get; set; } = new();

    /// <summary>排位单人对战</summary>
    public BattleStatisticsDto ClassSolo { get; set; } = new();

    /// <summary>排位组队对战</summary>
    public BattleStatisticsDto ClassTeam { get; set; } = new();

    /// <summary>使用过的机体（按对战场数降序）。</summary>
    public List<MobileSuitStatisticsDto> MobileSuits { get; set; } = new();
}
