using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Models.Cards.Titles.User;
using ServerInfinite.Persistence;
using ServerInfinite.Processor.Tracker;
using WebUIInfinite.Shared.Dto.Enum;

namespace ServerInfinite.Commands.LoadCard.MobileUser;

// Shared by the default and MS sticker fillers. Selector IDs 100/101/102 identify
// saved title slots; the wire text_id must be the selected title's resource ID.
public sealed class TitleTrackerResolver(ServerDbContext context, CardProfile cardProfile)
{
    private UserTitleSetting? _titles;

    public Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker? Create(uint selectorId)
    {
        if (selectorId != (uint)TrackerName.DefaultTitle && selectorId != (uint)TrackerName.SubTitle1
            && selectorId != (uint)TrackerName.SubTitle2)
            return null;

        _titles ??= context.UserTitleSettingDbSet
            .Include(x => x.UserDefaultTitle)
            .Include(x => x.UserSubTitle1)
            .Include(x => x.UserSubTitle2)
            .First(x => x.CardId == cardProfile.Id);

        UserTitle title = (TrackerName)selectorId switch
        {
            TrackerName.DefaultTitle => _titles.UserDefaultTitle,
            TrackerName.SubTitle1 => _titles.UserSubTitle1,
            TrackerName.SubTitle2 => _titles.UserSubTitle2,
            _ => throw new InvalidOperationException("Unknown title selector")
        };

        return new Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker
        {
            Type = TrackerProtocolTypes.Title,
            TextId = string.IsNullOrEmpty(title.CustomText) ? title.TitleTextId : 0,
            CustomText = title.CustomText
        };
    }
}
