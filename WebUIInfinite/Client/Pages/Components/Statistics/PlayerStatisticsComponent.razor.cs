using System.Globalization;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using WebUIInfinite.Client.Services.Mastery;
using WebUIInfinite.Client.Services.MS;
using WebUIInfinite.Client.Services.Name;
using WebUIInfinite.Shared.Dto.Common;
using WebUIInfinite.Shared.Dto.Statistics;
using WebUIInfinite.Shared.Resources;

namespace WebUIInfinite.Client.Pages.Components.Statistics;

/// <summary>
/// 卡片页底部的玩家战绩面板：四种对战模式的场次/胜场/胜率，以及每台使用过机体的场次/胜场/胜率/熟练度。
/// </summary>
public partial class PlayerStatisticsComponent
{
    [Parameter]
    public string AccessCode { get; set; } = string.Empty;

    [Parameter]
    public string ChipId { get; set; } = string.Empty;

    [Inject]
    private HttpClient Http { get; set; } = null!;

    [Inject]
    private IMobileSuitDataService MobileSuitDataService { get; set; } = null!;

    [Inject]
    private IFamiliarityDataService FamiliarityDataService { get; set; } = null!;

    [Inject]
    private INameService NameService { get; set; } = null!;

    [Inject]
    private IStringLocalizer<Resource> Localizer { get; set; } = null!;

    private bool _loading;
    private string? _errorMessage;
    private PlayerStatisticsDto? _statistics;

    private readonly List<ModeStatisticsRow> _modeRows = new();
    private readonly List<MobileSuitStatisticsRow> _mobileSuitRows = new();

    private string _loadedKey = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        if (string.IsNullOrWhiteSpace(AccessCode) || string.IsNullOrWhiteSpace(ChipId))
        {
            return;
        }

        var key = $"{AccessCode}|{ChipId}";
        if (key == _loadedKey)
        {
            return;
        }

        _loadedKey = key;
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _loading = true;
        _errorMessage = null;
        _statistics = null;
        _modeRows.Clear();
        _mobileSuitRows.Clear();

        try
        {
            var statistics = await Http.GetFromJsonAsync<PlayerStatisticsDto>(
                $"/ui/card/statistics/{AccessCode}/{ChipId}");

            if (statistics is null)
            {
                _errorMessage = Localizer["player_stats_load_failed"];
                return;
            }

            _statistics = statistics;

            _modeRows.Add(new ModeStatisticsRow("player_stats_offline_solo", statistics.OfflineSolo));
            _modeRows.Add(new ModeStatisticsRow("player_stats_offline_team", statistics.OfflineTeam));
            _modeRows.Add(new ModeStatisticsRow("player_stats_class_solo", statistics.ClassSolo));
            _modeRows.Add(new ModeStatisticsRow("player_stats_class_team", statistics.ClassTeam));

            BuildMobileSuitRows(statistics);
        }
        catch (Exception)
        {
            _errorMessage = Localizer["player_stats_load_failed"];
        }
        finally
        {
            _loading = false;
        }
    }

    private void BuildMobileSuitRows(PlayerStatisticsDto statistics)
    {
        foreach (var mobileSuitStatistics in statistics.MobileSuits)
        {
            var mobileSuit = MobileSuitDataService.GetMobileSuitById(mobileSuitStatistics.MstMobileSuitId);
            if (mobileSuit is null)
            {
                continue;
            }

            _mobileSuitRows.Add(new MobileSuitStatisticsRow
            {
                MstMobileSuitId = mobileSuitStatistics.MstMobileSuitId,
                Name = NameService.GetLocalizedName(mobileSuit) ?? mobileSuitStatistics.MstMobileSuitId.ToString(),
                BattleCount = mobileSuitStatistics.BattleCount,
                WinCount = mobileSuitStatistics.WinCount,
                WinRate = mobileSuitStatistics.WinRate,
                Mastery = FamiliarityDataService.ResolveMsFamiliarity(mobileSuitStatistics.MasteryPoint)
            });
        }
    }

    private static string FormatRate(double winRate)
    {
        return winRate.ToString("0.00", CultureInfo.InvariantCulture) + "%";
    }

    private sealed record ModeStatisticsRow(string LabelKey, BattleStatisticsDto Statistics);

    public sealed class MobileSuitStatisticsRow
    {
        public uint MstMobileSuitId { get; init; }
        public string Name { get; init; } = string.Empty;
        public uint BattleCount { get; init; }
        public uint WinCount { get; init; }
        public double WinRate { get; init; }
        public Familiarity Mastery { get; init; } = new();
    }
}
