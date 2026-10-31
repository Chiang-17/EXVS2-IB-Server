using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;
using ServerInfinite.Utils;

namespace ServerInfinite.Commands.PreLoadCard;

public class PrivateRoomCommand : IPreLoadCardCommand
{
    private readonly ServerDbContext _context;

    public PrivateRoomCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard preLoadCard)
    {
        var privateMatchRoomSetting = _context.PrivateMatchRoomSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        if (!privateMatchRoomSetting.EnablePrivateMatch)
        {
            return;
        }
        
        var participatedPrivateRoomId = privateMatchRoomSetting.ParticipatedPrivateRoomId;
        
        if (participatedPrivateRoomId == 0)
        {
            return;
        }

        var privateRoom = _context.PrivateMatchRoomDbSet
            .FirstOrDefault(x => x.Id == participatedPrivateRoomId);

        if (privateRoom is null)
        {
            return;
        }
        
        preLoadCard.matching_tag = new Response.PreLoadCard.MatchingTag()
        {
            Id = (uint) participatedPrivateRoomId,
            TagName = privateRoom.TagName,
            TagType = privateRoom.TagType, // 1 = Without MS Restriction, 2 = With MS Restriction
            MatchingType = privateRoom.MatchingType, // 1 = SOLO, 2 = TEAM
            MatchingAttribute = 1,
            EndDate = (ulong)DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds(),
            OncompeMatchingFlag = false,
            RuleType = privateRoom.RuleType,
            RevengeFlag = privateRoom.RevengeFlag
        };
        preLoadCard.matching_tag.SelectableMsId.AddRange(ArrayUtil.FromString(privateRoom.SelectableMsIds));
    }
}
