namespace WebUIInfinite.Shared.Dto.Common;

public class StickerDto
{
    public uint MobileSuitId { get; set; } = 0;
    
    public uint StickerBackgroundId { get; set; } = 0;
    public uint StickerEffectId { get; set; } = 0;

    /// <summary>Maps to PlayerSticker.Tracker.text_id (tag 3) for tracker slot 1–3.</summary>
    public uint Tracker1 { get; set; } = 0;
    public uint Tracker2 { get; set; } = 0;
    public uint Tracker3 { get; set; } = 0;

    /// <summary>Maps to PlayerSticker.pilot_image_disp_flag (tag 6). true = show pilot image.</summary>
    public bool PilotImageDispFlag { get; set; } = false;

    /// <summary>Maps to PlayerSticker.cutin_image_use_flag (tag 8). true = use cutin as pilot image. Only meaningful when <see cref="PilotImageDispFlag"/> is true.</summary>
    public bool CutinImageUseFlag { get; set; } = false;

    /// <summary>Maps to PlayerSticker.sub_background_id (tag 9).</summary>
    public uint SubBackgroundId { get; set; } = 0;

    // MS Exclusive
    /// <summary>Legacy single pose. Pose pool now lives in <see cref="PoseIds"/>.</summary>
    public uint PoseId { get; set; } = 0;

    /// <summary>Pose pool. Maps to PlayerSticker.pose_id (repeated, tag 2).</summary>
    public List<uint> PoseIds { get; set; } = new();

    /// <summary>Costume ids for pilot image. Maps to PlayerSticker.costume_id (repeated, tag 7).</summary>
    public List<uint> CostumeIds { get; set; } = new();
    
    // Default Exclusive
    public uint BasePanelId { get; set; } = 0;
    public uint CommentPartsAId { get; set; } = 0;
    public uint CommentPartsBId { get; set; } = 0;
}