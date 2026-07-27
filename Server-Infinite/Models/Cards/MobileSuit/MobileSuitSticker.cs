using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerInfinite.Models.Cards.MobileSuit;

[Table("exvs2ob_ms_sticker")]
[Index(nameof(StickerId))]
[Index(nameof(CardId))]
[Index(nameof(CardId), nameof(MstMobileSuitId))]
public class MobileSuitSticker : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StickerId { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint MstMobileSuitId { get; set; } = 0;

    /// <summary>Legacy single pose. Kept for backward compatibility; pose pool now lives in <see cref="PoseIds"/>.</summary>
    [Required]
    public uint PoseId { get; set; } = 0;

    /// <summary>Comma-separated pose ids. Maps to PlayerSticker.pose_id (repeated, tag 2).</summary>
    public string PoseIds { get; set; } = string.Empty;

    /// <summary>Comma-separated costume ids for pilot image. Maps to PlayerSticker.costume_id (repeated, tag 7).</summary>
    public string CostumeIds { get; set; } = string.Empty;

    /// <summary>Maps to PlayerSticker.pilot_image_disp_flag (tag 6). true = show pilot image.</summary>
    [Required]
    public bool PilotImageDispFlag { get; set; } = false;

    /// <summary>Maps to PlayerSticker.cutin_image_use_flag (tag 8). true = use cutin as pilot image.</summary>
    [Required]
    public bool CutinImageUseFlag { get; set; } = false;

    [Required]
    public uint StickerBackgroundId { get; set; } = 0;

    /// <summary>Maps to PlayerSticker.sub_background_id (tag 9).</summary>
    [Required]
    public uint SubBackgroundId { get; set; } = 0;

    [Required] 
    public uint StickerEffectId { get; set; } = 0;
    
    [Required] 
    public uint Tracker1 { get; set; } = 0;
    
    [Required] 
    public uint Tracker2 { get; set; } = 0;
    
    [Required] 
    public uint Tracker3 { get; set; } = 0;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}