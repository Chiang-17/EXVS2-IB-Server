using nue.protocol.exvs;
using ServerInfinite.Commands.LoadCard.MobileUser;
using ServerInfinite.Context.Tracker;
using ServerInfinite.Models.Cards.MobileSuit;
using ServerInfinite.Processor.Tracker.Rarity;
using ProtoTracker = nue.protocol.exvs.Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker;

namespace ServerInfinite.Processor.Tracker;

public sealed class StickerTrackerFactory(
    ITrackerCatalog catalog, PilotTrackerContext pilot, TitleTrackerResolver titles,
    IReadOnlyDictionary<uint, MobileSuitPvPStatistic> mobileStats,
    IReadOnlyDictionary<uint, uint> mobileSuitUsage)
{
    private readonly IReadOnlyDictionary<uint, WebUIInfinite.Shared.Dto.Common.TrackerType> _definitions = catalog.Load();
    private readonly IReadOnlyList<WebUIInfinite.Shared.Dto.Common.Familiarity> _familiarities = catalog.LoadMobileSuitFamiliarities();

    public ProtoTracker? Create(uint id, uint mobileSuitId = 0)
    {
        if (!_definitions.TryGetValue(id, out var definition)) return null;
        if (definition.Type == TrackerProtocolTypes.Title)
        {
            var title = titles.Create(id) ?? new ProtoTracker { Type = TrackerProtocolTypes.Title, TextId = 0, CustomText = string.Empty };
            if (definition.Threshold is { Length: > 0 })
                title.Rarity = StatisticRarity.Calculate(pilot.PlayerLevel, definition.Threshold);
            return title;
        }

        var tracker = new ProtoTracker { Type = definition.Type, TextId = id, CustomText = string.Empty };
        if (definition.Type == TrackerProtocolTypes.Empty) return tracker; // Preserve empty slot positions.
        var ms = mobileStats.GetValueOrDefault(mobileSuitId);
        double value = id switch
        {
            1 => pilot.TotalBattleCount,
            2 => pilot.TotalWinCount,
            3 => pilot.TotalGivenDamage,
            4 => pilot.TotalEnemyDefeatedCount,
            5 => pilot.TotalClassMatchTenConsecutiveWinCount,
            6 => pilot.TotalNoDamageBattleCount,
            7 => pilot.TotalExBurstDamage,
            8 => WinRate(pilot.TotalWinCount, pilot.TotalBattleCount),
            9 => pilot.PlayerLevel,
            10 => pilot.SoloClassRate,
            11 => pilot.TeamClassRate,
            12 => ms?.TotalBattleCount ?? 0,
            13 => ms?.TotalWinCount ?? 0,
            14 => ms?.TotalGivenDamage ?? 0,
            15 => ms?.TotalEnemyDefeatedCount ?? 0,
            16 => ms?.TotalClassMatchTenConsecutiveWinCount ?? 0,
            17 => ms?.TotalNoDamageBattleCount ?? 0,
            18 => ms?.TotalExBurstDamage ?? 0,
            19 => WinRate(ms?.TotalWinCount ?? 0, ms?.TotalBattleCount ?? 0),
            20 => pilot.ObBattleCount,
            21 => pilot.LicenseScore,
            22 => pilot.ClassMatchDailyWinRate,
            23 => pilot.TotalPlayDays,
            25 => pilot.MobileSuitDailyWinRates.GetValueOrDefault(mobileSuitId),
            _ => 0
        };
        if (!double.IsFinite(value) || value < 0) value = 0;
        // These are the client's floating-point label IDs (9C9810). Rates 10/11
        // use the integer display; truncate their stored fractional portion.
        if (id is 8 or 19 or 22 or 25)
            tracker.FloatTrackerValue = (float)Math.Min(value, float.MaxValue);
        else
            tracker.IntgerTrackerValue = (uint)Math.Min(value, uint.MaxValue);
        // Only these statistic types use rarity to select their frame style.
        // External 0/1 are player statistics, 2/3 are MS statistics; 7 is OB.
        if (definition.Type is 0 or 1 or 2 or 3 or 7 && definition.Threshold is { Length: > 0 })
        {
            // Mastery uses the catalog rank ID (e.g. star 22), not its repeating
            // within-color Level (1–5), nor the raw number of MS uses.
            var rarityLevel = definition.Type is 2 or 3
                ? _familiarities.Where(x => x.MinimumPoint <= mobileSuitUsage.GetValueOrDefault(mobileSuitId))
                    .OrderByDescending(x => x.MinimumPoint).Select(x => x.Id).FirstOrDefault()
                : pilot.PlayerLevel;
            tracker.Rarity = StatisticRarity.Calculate(rarityLevel, definition.Threshold);
        }
        return tracker;
    }

    private static double WinRate(uint wins, uint battles) => battles == 0 ? 0 : 100d * wins / battles;
}
