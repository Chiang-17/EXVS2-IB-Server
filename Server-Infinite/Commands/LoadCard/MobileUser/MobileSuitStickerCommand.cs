using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Models.Cards.Battle;
using ServerInfinite.Models.Cards.MobileSuit;
using ServerInfinite.Persistence;
using ServerInfinite.Processor.Tracker;

namespace ServerInfinite.Commands.LoadCard.MobileUser;

public class MobileSuitStickerCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;
    private readonly IStickerTrackerProcessor _mobileSuitStickerTrackerProcessor;

    public MobileSuitStickerCommand(ServerDbContext context, IStickerTrackerProcessor mobileSuitStickerTrackerProcessor)
    {
        _context = context;
        _mobileSuitStickerTrackerProcessor = mobileSuitStickerTrackerProcessor;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var playerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == cardProfile);
        
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

            AddTracker(msPlayerSticker, mobileSuitSticker.Tracker1,
                cardProfile, mobileSuitSticker, playerLevel);
            AddTracker(msPlayerSticker, mobileSuitSticker.Tracker2,
                cardProfile, mobileSuitSticker, playerLevel);
            AddTracker(msPlayerSticker, mobileSuitSticker.Tracker3,
                cardProfile, mobileSuitSticker, playerLevel);
            
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

    void AddTracker(
        Response.LoadCard.MobileUserGroup.PlayerSticker sticker,
        uint textId,
        CardProfile cardProfile,
        MobileSuitSticker mobileSuitSticker,
        PlayerLevel playerLevel)
    {
        // No tracker selected for this slot: skip entirely (do not add to the Tracker array).
        if (textId == 0)
        {
            return;
        }

        var tracker = new Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker
        {
            TextId = textId,
            CustomText = string.Empty,
        };

        // Look up rarity / value from text_id against real MS (falling back to pilot) stats.
        ProcessMobileSuitTracker(tracker, cardProfile, mobileSuitSticker, playerLevel);

        // tracker type is determined by the text_id's Category (not the value lookup).
        tracker.Type = Constants.TrackerTypes.ResolveTypeByTextId(textId);

        sticker.Trackers.Add(tracker);
    }

    void ProcessMobileSuitTracker(Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker tracker, 
        CardProfile cardProfile, MobileSuitSticker mobileSuitSticker, PlayerLevel playerLevel)
    {
        var mobilePvPStat = _context.MobileSuitPvPStatisticDbSet
            .FirstOrDefault(x =>
                x.CardProfile == cardProfile &&
                x.MstMobileSuitId == mobileSuitSticker.MstMobileSuitId
            );

        // Pass through (possibly null) MS stat; MobileSuitTrackerProcessor returns early when null.
        _mobileSuitStickerTrackerProcessor.Process(tracker, mobilePvPStat, playerLevel);
    }
}