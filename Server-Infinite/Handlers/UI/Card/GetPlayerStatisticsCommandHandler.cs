using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;
using WebUIInfinite.Shared.Dto.Statistics;
using WebUIInfinite.Shared.Exception;

namespace ServerInfinite.Handlers.UI.Card;

public record GetPlayerStatisticsCommand(string AccessCode, string ChipId) : IRequest<PlayerStatisticsDto>;

/// <summary>
/// 汇总卡片在“修改卡信息 / 显示玩家资料”页底部展示的战绩：
/// 店内单人、店内组队、排位单人、排位组队，以及每台使用过的机体的场数/胜场/胜率/熟练度。
/// </summary>
public class GetPlayerStatisticsCommandHandler : IRequestHandler<GetPlayerStatisticsCommand, PlayerStatisticsDto>
{
    private readonly ServerDbContext _context;

    public GetPlayerStatisticsCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<PlayerStatisticsDto> Handle(GetPlayerStatisticsCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = _context.CardProfiles
            .Include(x => x.MobileSuits)
            .Include(x => x.MobileSuitPvPStatistics)
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        // 老卡片可能没有对应的战绩行，缺失时按全 0 处理，避免整页 500。
        var winLossRecord = _context.WinLossRecordDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile);

        var playerStatistics = new PlayerStatisticsDto
        {
            OfflineSolo = winLossRecord is null ? new BattleStatisticsDto() : ToStatistics(winLossRecord.ShuffleWin, winLossRecord.ShuffleLose),
            OfflineTeam = winLossRecord is null ? new BattleStatisticsDto() : ToStatistics(winLossRecord.TeamWin, winLossRecord.TeamLose),
            ClassSolo = winLossRecord is null ? new BattleStatisticsDto() : ToStatistics(winLossRecord.ClassSoloWin, winLossRecord.ClassSoloLose),
            ClassTeam = winLossRecord is null ? new BattleStatisticsDto() : ToStatistics(winLossRecord.ClassTeamWin, winLossRecord.ClassTeamLose),
            MobileSuits = BuildMobileSuitStatistics(cardProfile)
        };

        return Task.FromResult(playerStatistics);
    }

    private static List<MobileSuitStatisticsDto> BuildMobileSuitStatistics(CardProfile cardProfile)
    {
        var pvpStatistics = cardProfile.MobileSuitPvPStatistics
            .ToDictionary(x => x.MstMobileSuitId);

        var usages = cardProfile.MobileSuits
            .ToDictionary(x => x.MstMobileSuitId);

        // 机体可能只在其中一张表里出现（例如只打过练习/训练），取并集后再过滤。
        var mobileSuitIds = pvpStatistics.Keys
            .Union(usages.Keys);

        var mobileSuitStatistics = new List<MobileSuitStatisticsDto>();

        foreach (var mobileSuitId in mobileSuitIds)
        {
            pvpStatistics.TryGetValue(mobileSuitId, out var pvpStatistic);
            usages.TryGetValue(mobileSuitId, out var usage);

            var battleCount = pvpStatistic?.TotalBattleCount ?? 0;
            var winCount = pvpStatistic?.TotalWinCount ?? 0;
            var masteryPoint = usage?.MsUsedNum ?? 0;

            if (battleCount == 0 && masteryPoint == 0)
            {
                continue;
            }

            if (winCount > battleCount)
            {
                winCount = battleCount;
            }

            mobileSuitStatistics.Add(new MobileSuitStatisticsDto
            {
                MstMobileSuitId = mobileSuitId,
                BattleCount = battleCount,
                WinCount = winCount,
                LoseCount = battleCount - winCount,
                WinRate = CalculateWinRate(battleCount, winCount),
                MasteryPoint = masteryPoint
            });
        }

        return mobileSuitStatistics
            .OrderByDescending(x => x.BattleCount)
            .ThenByDescending(x => x.WinCount)
            .ThenBy(x => x.MstMobileSuitId)
            .ToList();
    }

    private static BattleStatisticsDto ToStatistics(uint winCount, uint loseCount)
    {
        var battleCount = winCount + loseCount;

        return new BattleStatisticsDto
        {
            BattleCount = battleCount,
            WinCount = winCount,
            LoseCount = loseCount,
            WinRate = CalculateWinRate(battleCount, winCount)
        };
    }

    private static double CalculateWinRate(uint battleCount, uint winCount)
    {
        if (battleCount == 0)
        {
            return 0;
        }

        return Math.Round(100.0 * winCount / battleCount, 2);
    }
}
