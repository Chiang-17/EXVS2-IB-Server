using ServerInfinite.Mapper.Card.Battle;
using WebUIInfinite.Shared.Dto.Enum;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;

namespace ServerInfinite.Context.Tracker;

public class PilotTrackerContextGenerator
{
    private readonly ServerDbContext _context;

    public PilotTrackerContextGenerator(ServerDbContext context)
    {
        _context = context;
    }
    
    public PilotTrackerContext Generate(CardProfile cardProfile)
    {
        var playerBattleStatistic = _context.PlayerBattleStatisticDbSet
            .FirstOrDefault(x => x.CardId == cardProfile.Id);
        
        var winLossRecord = _context.WinLossRecordDbSet
            .FirstOrDefault(x => x.CardId == cardProfile.Id);
        
        var playerLevel = _context.PlayerLevelDbSet
            .FirstOrDefault(x => x.CardId == cardProfile.Id);
        
        var soloClassMatchRecord = _context.SoloClassMatchRecordDbSet
            .FirstOrDefault(x => x.CardId == cardProfile.Id);
        
        var teamClassMatchRecord = _context.TeamClassMatchRecordDbSet
            .FirstOrDefault(x => x.CardId == cardProfile.Id);
        
        var pilotTrackerContext = playerBattleStatistic?.ToPilotTrackerContext() ?? new PilotTrackerContext();

        pilotTrackerContext.TotalBattleCount = (uint)Math.Min((ulong)(winLossRecord?.TotalWin ?? 0) + (winLossRecord?.TotalLose ?? 0), uint.MaxValue);
        pilotTrackerContext.TotalWinCount = winLossRecord?.TotalWin ?? 0;
        pilotTrackerContext.PlayerLevel = playerLevel?.PlayerLevelId ?? 0;
        pilotTrackerContext.SoloClassRate = soloClassMatchRecord?.Rate ?? 0;
        pilotTrackerContext.TeamClassRate = teamClassMatchRecord?.Rate ?? 0;

        pilotTrackerContext.LicenseScore = _context.LicenseScoreRecordDbSet
            .FirstOrDefault(x => x.CardId == cardProfile.Id)?.LicenseScore ?? 0;

        // Daily means the server's local calendar day; both solo and team class matches count.
        var start = new DateTimeOffset(DateTime.Today).ToUnixTimeSeconds();
        var end = new DateTimeOffset(DateTime.Today.AddDays(1)).ToUnixTimeSeconds();
        var today = _context.BattleHistoryDbSet
            .Where(x => x.CardId == cardProfile.Id && (long)x.PlayedAt >= start && (long)x.PlayedAt < end
                && (x.BattleMode == BattleModeConstant.ClassMatchSolo || x.BattleMode == BattleModeConstant.ClassMatchTeam))
            .Select(x => new { x.IsWin, x.BattleSelf.MobileSuitId }).ToList();
        pilotTrackerContext.ClassMatchDailyWinRate = today.Count == 0 ? 0f : 100f * today.Count(x => x.IsWin) / today.Count;
        pilotTrackerContext.MobileSuitDailyWinRates = today.GroupBy(x => x.MobileSuitId)
            .ToDictionary(x => x.Key, x => 100f * x.Count(b => b.IsWin) / x.Count());
        // Do not substitute current-version battles for OB battles or PvP-only days for all play days.
        return pilotTrackerContext;
    }
}