namespace WebUIInfinite.Shared.Dto.Common;

/// <summary>
/// Ensures pilot costume/pose pools have a valid wire value on save.
/// Costume default applies only when pilot image display is enabled; pose default always applies.
/// </summary>
public static class StickerPilotPoolDefaults
{
    public const uint DefaultPoolId = 0;

    /// <summary>
    /// Empty costume pool is persisted as Default when <see cref="StickerDto.PilotImageDispFlag"/> is true.
    /// Empty pose pool is always persisted as a single Default entry (id 0).
    /// </summary>
    public static void ApplyEmptyPoolDefaultsOnSave(StickerDto sticker)
    {
        if (sticker.PilotImageDispFlag &&
            (sticker.CostumeIds is null || sticker.CostumeIds.Count == 0))
        {
            sticker.CostumeIds = new List<uint> { DefaultPoolId };
        }

        if (sticker.PoseIds is null || sticker.PoseIds.Count == 0)
        {
            sticker.PoseIds = new List<uint> { DefaultPoolId };
            sticker.PoseId = DefaultPoolId;
        }
        else
        {
            sticker.PoseId = sticker.PoseIds.FirstOrDefault();
        }
    }
}
