using nue.protocol.exvs;
using ServerInfinite.Mapper.Card.QuickStart;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;

namespace ServerInfinite.Commands.Common;

public class SaveQuickStartSettingCommand
{
    private readonly ServerDbContext _context;

    public SaveQuickStartSettingCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Save(CardProfile cardProfile, QuickStartInfo? quickStartInfo)
    {
        if (quickStartInfo is null)
            return;

        var setting = _context.QuickStartSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        QuickStartInfoMapper.ApplyFromProto(setting, quickStartInfo);
    }
}
