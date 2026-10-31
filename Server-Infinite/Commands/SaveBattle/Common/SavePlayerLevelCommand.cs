using ServerInfinite.Context.Battle;
using ServerInfinite.Models.Cards;
using ServerInfinite.Models.Cards.Battle;
using ServerInfinite.Persistence;

namespace ServerInfinite.Commands.SaveBattle.Common;

public class SavePlayerLevelCommand : ISaveBattleDataCommand
{
    private readonly ServerDbContext _context;

    public SavePlayerLevelCommand(ServerDbContext context)
    {
        _context = context;
    }
    
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var playerLevelDomain = battleResultContext.PlayerLevelDomain;
        var playerLevelData = _context.PlayerLevelDbSet.First(x => x.CardProfile == cardProfile);
        // The current client owns the level value. Do not apply legacy round
        // offsets, caps, resets, or comparisons against a locally inferred level.
        if (playerLevelDomain.LevelIdBefore == playerLevelDomain.LevelIdAfter)
            playerLevelData.PlayerExp += playerLevelDomain.ExpIncrement;
        else
            playerLevelData.PlayerExp = 0;
        playerLevelData.PlayerLevelId = playerLevelDomain.LevelIdAfter;
        playerLevelData.LevelMaxDispFlag = false;
    }
}
