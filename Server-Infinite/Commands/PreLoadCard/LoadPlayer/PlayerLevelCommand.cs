using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;

namespace ServerInfinite.Commands.PreLoadCard.LoadPlayer;

public class PlayerLevelCommand : IPreLoadPlayerCommand
{
    private readonly ServerDbContext _context;

    public PlayerLevelCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        var playerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == cardProfile);

        loadPlayer.PlayerLevelId = playerLevel.PlayerLevelId;
        // Badge data belongs to PreLoadCard.MobileUserGroup in revision 141.
        loadPlayer.PlayerExp = playerLevel.PlayerExp;
    }
}
