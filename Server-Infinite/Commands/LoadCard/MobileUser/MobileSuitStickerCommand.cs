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
    private readonly IStickerTrackerProcessor _pilotTrackerProcessor;
    private readonly IStickerTrackerProcessor _mobileSuitStickerTrackerProcessor;

    public MobileSuitStickerCommand(ServerDbContext context, IStickerTrackerProcessor pilotTrackerProcessor, IStickerTrackerProcessor mobileSuitStickerTrackerProcessor)
    {
        _context = context;
        _pilotTrackerProcessor = pilotTrackerProcessor;
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

            AddTracker(msPlayerSticker, mobileSuitSticker.Tracker1Type, mobileSuitSticker.Tracker1, mobileSuitSticker.Tracker1CustomText,
                cardProfile, mobileSuitSticker, playerLevel);
            AddTracker(msPlayerSticker, mobileSuitSticker.Tracker2Type, mobileSuitSticker.Tracker2, mobileSuitSticker.Tracker2CustomText,
                cardProfile, mobileSuitSticker, playerLevel);
            AddTracker(msPlayerSticker, mobileSuitSticker.Tracker3Type, mobileSuitSticker.Tracker3, mobileSuitSticker.Tracker3CustomText,
                cardProfile, mobileSuitSticker, playerLevel);
            
            mobileUserGroup.PlayerStickers.Add(msPlayerSticker);
        });
    }

    bool HasBlankSticker(MobileSuitSticker mobileSuitSticker)
    {
        var isBlankTracker = mobileSuitSticker.Tracker1 == 0 && mobileSuitSticker.Tracker2 == 0 &&
                             mobileSuitSticker.Tracker3 == 0 &&
                             mobileSuitSticker.Tracker1Type == 0 && mobileSuitSticker.Tracker2Type == 0 &&
                             mobileSuitSticker.Tracker3Type == 0;
        var isBlankDisplay = mobileSuitSticker.StickerBackgroundId == 0 && mobileSuitSticker.StickerEffectId == 0 &&
                             mobileSuitSticker.SubBackgroundId == 0;
        var isWithoutPose = mobileSuitSticker.PoseId == 0 &&
                            Mapper.Card.Sticker.StickerDtoMapping.ParseCsv(mobileSuitSticker.PoseIds).Count == 0;
        var isWithoutPilotImage = !mobileSuitSticker.PilotImageDispFlag;

        return isBlankTracker && isBlankDisplay && isWithoutPose && isWithoutPilotImage;
    }

    void AddTracker(
        Response.LoadCard.MobileUserGroup.PlayerSticker sticker,
        uint type,
        uint textId,
        string customText,
        CardProfile cardProfile,
        MobileSuitSticker mobileSuitSticker,
        PlayerLevel playerLevel)
    {
        // Open-custom tracker: user directly controls type / text_id / custom_text.
        // Skip only when the whole slot is blank.
        if (type == 0 && textId == 0 && string.IsNullOrEmpty(customText))
        {
            return;
        }

        var tracker = new Response.LoadCard.MobileUserGroup.PlayerSticker.Tracker
        {
            Type = type,
            TextId = textId,
            CustomText = customText ?? string.Empty
        };

        // Look up rarity / tracker value (and mapped type) from text_id against real stats.
        ProcessMobileSuitTracker(tracker, cardProfile, mobileSuitSticker, playerLevel);

        // Open-custom: preserve the user-provided type when set; otherwise keep the mapped type.
        if (type != 0)
        {
            tracker.Type = type;
        }

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

        if (mobilePvPStat is null)
        {
            _pilotTrackerProcessor.Process(tracker, null, playerLevel);
            return;
        }
        
        _mobileSuitStickerTrackerProcessor.Process(tracker, mobilePvPStat, playerLevel);
    }
}