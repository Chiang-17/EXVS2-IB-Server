using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Models.Cards.MobileSuit;
using ServerInfinite.Persistence;
using ServerInfinite.Processor.Tracker;

namespace ServerInfinite.Commands.LoadCard.MobileUser;

public class MobileSuitStickerCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;
    private readonly StickerTrackerFactory _trackers;

    public MobileSuitStickerCommand(ServerDbContext context, StickerTrackerFactory trackers)
    {
        _context = context;
        _trackers = trackers;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var mobileSuitStickers = _context.MobileSuitStickerDbSet
            .Where(x => x.CardProfile == cardProfile)
            .OrderBy(x => x.MstMobileSuitId)
            .ToList();
        
        mobileSuitStickers.ForEach(mobileSuitSticker =>
        {
            if (HasBlankSticker(mobileSuitSticker))
            {
                return;
            }
            
            var poseIds = Mapper.Card.Sticker.StickerDtoMapping.ParseCsv(mobileSuitSticker.PoseIds);
            if (poseIds.Count == 0 && mobileSuitSticker.PoseId > 0)
            {
                poseIds.Add(mobileSuitSticker.PoseId);
            }

            var costumeIds = mobileSuitSticker.PilotImageDispFlag
                ? Mapper.Card.Sticker.StickerDtoMapping.ParseCsv(mobileSuitSticker.CostumeIds)
                : new List<uint>();
            if (mobileSuitSticker.PilotImageDispFlag && mobileSuitSticker.CutinImageUseFlag && costumeIds.Count == 0)
                costumeIds.Add(0); // Client requires a costume candidate to enable cutin.

            var msPlayerSticker = new Response.LoadCard.MobileUserGroup.PlayerSticker()
            {
                MstMobileSuitId = mobileSuitSticker.MstMobileSuitId,
                PoseIds = poseIds.ToArray(),
                CostumeIds = costumeIds.ToArray(),
                PilotImageDispFlag = mobileSuitSticker.PilotImageDispFlag,
                CutinImageUseFlag = mobileSuitSticker.PilotImageDispFlag && mobileSuitSticker.CutinImageUseFlag,
                StickerBackgroundId = mobileSuitSticker.StickerBackgroundId,
                SubBackgroundId = mobileSuitSticker.SubBackgroundId,
                StickerEffectId = mobileSuitSticker.StickerEffectId,
            };

            foreach (var id in new[] { mobileSuitSticker.Tracker1, mobileSuitSticker.Tracker2, mobileSuitSticker.Tracker3 })
                if (_trackers.Create(id, mobileSuitSticker.MstMobileSuitId) is { } tracker) msPlayerSticker.Trackers.Add(tracker);

            mobileUserGroup.PlayerStickers.Add(msPlayerSticker);
        });
    }

    bool HasBlankSticker(MobileSuitSticker mobileSuitSticker)
    {
        var isBlankTracker = mobileSuitSticker.Tracker1 == 0 && mobileSuitSticker.Tracker2 == 0 &&
                             mobileSuitSticker.Tracker3 == 0;
        var isBlankDisplay = mobileSuitSticker.StickerBackgroundId == 0 && mobileSuitSticker.StickerEffectId == 0 &&
                             mobileSuitSticker.SubBackgroundId == 0;
        var isWithoutPose = mobileSuitSticker.PoseId == 0 &&
                            Mapper.Card.Sticker.StickerDtoMapping.ParseCsv(mobileSuitSticker.PoseIds).Count == 0;
        var isWithoutPilotImage = !mobileSuitSticker.PilotImageDispFlag;

        return isBlankTracker && isBlankDisplay && isWithoutPose && isWithoutPilotImage;
    }

}
