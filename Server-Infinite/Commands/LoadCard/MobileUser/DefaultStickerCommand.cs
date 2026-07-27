using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;
using ServerInfinite.Processor.Tracker;

namespace ServerInfinite.Commands.LoadCard.MobileUser;

public class DefaultStickerCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;
    private readonly IStickerTrackerProcessor _stickerTrackerProcessor;

    public DefaultStickerCommand(ServerDbContext context, IStickerTrackerProcessor stickerTrackerProcessor)
    {
        _context = context;
        _stickerTrackerProcessor = stickerTrackerProcessor;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var defaultStickerProfile = _context.DefaultStickerProfileDbSet
            .First(x => x.CardProfile == cardProfile);
        
        var defaultSticker = new Response.LoadCard.MobileUserGroup.PlayerSticker()
        {
            MstMobileSuitId = 0,
            PoseIds = global::System.Array.Empty<uint>(),
            CostumeIds = global::System.Array.Empty<uint>(),
            PilotImageDispFlag = defaultStickerProfile.PilotImageDispFlag,
            CutinImageUseFlag = defaultStickerProfile.PilotImageDispFlag && defaultStickerProfile.CutinImageUseFlag,
            StickerBackgroundId = defaultStickerProfile.StickerBackgroundId,
            StickerEffectId = defaultStickerProfile.StickerEffectId,
            // Legacy server-only fields retained for WebUI editing round-trip.
            BasePanelId = defaultStickerProfile.BasePanelId,
            CommentPartsAId = defaultStickerProfile.CommentPartsAId,
            CommentPartsBId = defaultStickerProfile.CommentPartsBId,
        };

        var playerLevel = _context.PlayerLevelDbSet
            .First(x => x.CardProfile == cardProfile);
        
        AddTracker(defaultSticker, defaultStickerProfile.Tracker1, playerLevel);
        AddTracker(defaultSticker, defaultStickerProfile.Tracker2, playerLevel);
        AddTracker(defaultSticker, defaultStickerProfile.Tracker3, playerLevel);
        
        mobileUserGroup.PlayerStickers.Add(defaultSticker);
    }

    void AddTracker(
        Response.LoadCard.MobileUserGroup.PlayerSticker sticker,
        uint textId,
        Models.Cards.Battle.PlayerLevel playerLevel)
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

        // Look up rarity / value from text_id against real player stats.
        _stickerTrackerProcessor.Process(tracker, null, playerLevel);

        // tracker type is determined by the text_id's Category (not the value lookup).
        tracker.Type = Constants.TrackerTypes.ResolveTypeByTextId(textId);

        sticker.Trackers.Add(tracker);
    }
}