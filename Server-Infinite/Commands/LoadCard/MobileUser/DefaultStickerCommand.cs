using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;
using ServerInfinite.Processor.Tracker;

namespace ServerInfinite.Commands.LoadCard.MobileUser;

public class DefaultStickerCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;
    private readonly StickerTrackerFactory _trackers;

    public DefaultStickerCommand(ServerDbContext context, StickerTrackerFactory trackers)
    {
        _context = context;
        _trackers = trackers;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var defaultStickerProfile = _context.DefaultStickerProfileDbSet
            .First(x => x.CardProfile == cardProfile);
        
        var defaultSticker = new Response.LoadCard.MobileUserGroup.PlayerSticker()
        {
            MstMobileSuitId = 0,
            PoseIds = global::System.Array.Empty<uint>(),
            // Client 7AAA40 disables cutin when the costume pool is empty.
            // A single default costume makes the requested cutin deterministic.
            CostumeIds = defaultStickerProfile.PilotImageDispFlag && defaultStickerProfile.CutinImageUseFlag
                ? new uint[] { 0 } : global::System.Array.Empty<uint>(),
            PilotImageDispFlag = defaultStickerProfile.PilotImageDispFlag,
            CutinImageUseFlag = defaultStickerProfile.PilotImageDispFlag && defaultStickerProfile.CutinImageUseFlag,
            StickerBackgroundId = defaultStickerProfile.StickerBackgroundId,
            SubBackgroundId = defaultStickerProfile.SubBackgroundId,
            StickerEffectId = defaultStickerProfile.StickerEffectId,
            // Legacy server-only fields retained for WebUI editing round-trip.
            BasePanelId = defaultStickerProfile.BasePanelId,
            CommentPartsAId = defaultStickerProfile.CommentPartsAId,
            CommentPartsBId = defaultStickerProfile.CommentPartsBId,
        };

        foreach (var id in new[] { defaultStickerProfile.Tracker1, defaultStickerProfile.Tracker2, defaultStickerProfile.Tracker3 })
            if (_trackers.Create(id) is { } tracker) defaultSticker.Trackers.Add(tracker);

        mobileUserGroup.PlayerStickers.Add(defaultSticker);
    }

}
