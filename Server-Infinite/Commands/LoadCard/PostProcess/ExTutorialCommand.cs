using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;

namespace ServerInfinite.Commands.LoadCard.PostProcess;

public class ExTutorialCommand : ILoadCardPostProcessCommand
{
    private readonly ServerDbContext _context;
    
    public ExTutorialCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void PostProcess(CardProfile cardProfile, Request.LoadCard loadCardRequest)
    {
        if (!loadCardRequest.ShouldSerializeExTutorialDispFlag())
        {
            return;
        }

        if (!loadCardRequest.ExTutorialDispFlag)
        {
            return;
        }

        var playerProfile = _context.PlayerProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        playerProfile.ExTutorialDispFlag = false;

        _context.SaveChanges();
    }
}