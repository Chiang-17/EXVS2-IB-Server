using ServerInfinite.Models.Cards.MobileSuit;
using ServerInfinite.Models.Cards.Profile;
using WebUIInfinite.Shared.Dto.Common;

namespace ServerInfinite.Mapper.Card.Sticker;

public static class StickerDtoMapping
{
    public static StickerDto ToStickerDto(this MobileSuitSticker sticker)
    {
        var poseIds = ParseCsv(sticker.PoseIds);
        if (poseIds.Count == 0 && sticker.PoseId > 0)
        {
            poseIds.Add(sticker.PoseId);
        }

        return new StickerDto
        {
            MobileSuitId = sticker.MstMobileSuitId,
            StickerBackgroundId = sticker.StickerBackgroundId,
            SubBackgroundId = sticker.SubBackgroundId,
            StickerEffectId = sticker.StickerEffectId,
            Tracker1 = sticker.Tracker1,
            Tracker2 = sticker.Tracker2,
            Tracker3 = sticker.Tracker3,
            PilotImageDispFlag = sticker.PilotImageDispFlag,
            CutinImageUseFlag = sticker.CutinImageUseFlag,
            PoseId = poseIds.Count > 0 ? poseIds[0] : 0,
            PoseIds = poseIds,
            CostumeIds = ParseCsv(sticker.CostumeIds),
        };
    }

    public static StickerDto ToStickerDto(this DefaultStickerProfile sticker)
    {
        return new StickerDto
        {
            MobileSuitId = 0,
            StickerBackgroundId = sticker.StickerBackgroundId,
            StickerEffectId = sticker.StickerEffectId,
            Tracker1 = sticker.Tracker1,
            Tracker2 = sticker.Tracker2,
            Tracker3 = sticker.Tracker3,
            PilotImageDispFlag = sticker.PilotImageDispFlag,
            CutinImageUseFlag = sticker.CutinImageUseFlag,
            BasePanelId = sticker.BasePanelId,
            CommentPartsAId = sticker.CommentPartsAId,
            CommentPartsBId = sticker.CommentPartsBId,
        };
    }

    public static List<uint> ParseCsv(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
        {
            return new List<uint>();
        }

        return csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(token => uint.TryParse(token, out var value) ? value : (uint?)null)
            .Where(value => value.HasValue)
            .Select(value => value!.Value)
            .Distinct()
            .ToList();
    }

    public static string ToCsv(IEnumerable<uint>? values)
    {
        if (values is null)
        {
            return string.Empty;
        }

        return string.Join(",", values.Distinct());
    }
}
