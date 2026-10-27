using ServerInfinite.Models.Cards;
using ServerInfinite.Models.Cards.Battle;
using ServerInfinite.Models.Config;
using ServerInfinite.Persistence;

namespace ServerInfinite.Commands.SaveBattle.Common;

/// <summary>
/// Badge (route battle level) persistence.
/// - obtained_badge_point (SaveVscResult 25 / SaveVsmOnResult 69, sint32, may be negative) is added to BadgeExp.
/// - badge_id reported by the game (SaveVsmOnResult 19 / SaveUserPlayResearchData 6) is stored as BadgeId.
///   The game decides the up/down itself; on a change BadgeExp is optionally reset to the
///   configured up/down default exp (same idea as LoadGameData.BadgeExpTable).
/// Changes are tracked on the DbContext; the caller calls SaveChanges.
/// </summary>
public class SaveBadgeCommand
{
    private readonly ServerDbContext _context;
    private readonly RouteBattleConfigurations _config;
    private readonly ILogger? _logger;

    public SaveBadgeCommand(ServerDbContext context, CardServerConfig? config = null, ILogger? logger = null)
    {
        _context = context;
        _config = config?.GameConfigurations?.RouteBattleConfigurations ?? new RouteBattleConfigurations();
        _logger = logger;
    }

    private PlayerLevel? Find(CardProfile cardProfile)
    {
        var cardId = cardProfile.Id;
        var tracked = _context.PlayerLevelDbSet.Local.FirstOrDefault(x => x.CardId == cardId);
        return tracked ?? _context.PlayerLevelDbSet.FirstOrDefault(x => x.CardId == cardId);
    }

    /// <summary>BadgeExp += obtainedBadgePoint.</summary>
    public void Save(CardProfile cardProfile, int obtainedBadgePoint)
    {
        if (obtainedBadgePoint == 0)
        {
            return;
        }

        var playerLevel = Find(cardProfile);
        if (playerLevel == null)
        {
            return;
        }

        var before = playerLevel.BadgeExp;
        playerLevel.BadgeExp += obtainedBadgePoint;
        _logger?.LogInformation("Badge exp CardId={CardId} badge={BadgeId} exp {Before} -> {After} ({Delta:+#;-#;0})",
            cardProfile.Id, playerLevel.BadgeId, before, playerLevel.BadgeExp, obtainedBadgePoint);
    }

    /// <summary>Store the badge_id reported by the game.</summary>
    public void ApplyReportedBadge(CardProfile cardProfile, uint reportedBadgeId, string source)
    {
        var playerLevel = Find(cardProfile);
        if (playerLevel == null || playerLevel.BadgeId == reportedBadgeId)
        {
            return;
        }

        var oldId = playerLevel.BadgeId;
        var oldExp = playerLevel.BadgeExp;
        playerLevel.BadgeId = reportedBadgeId;
        if (_config.ResetBadgeExpOnChange)
        {
            playerLevel.BadgeExp = reportedBadgeId > oldId ? _config.BadgeUpDefaultExp : _config.BadgeDownDefaultExp;
        }

        _logger?.LogInformation("Badge {Direction} CardId={CardId} ({Source}): badge {OldId} -> {NewId}, exp {OldExp} -> {NewExp}",
            reportedBadgeId > oldId ? "UP" : "DOWN", cardProfile.Id, source, oldId, reportedBadgeId, oldExp, playerLevel.BadgeExp);
    }
}
