using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerInfinite.Models.Cards.Profile;

[Table("exvs2ob_default_sticker_profile")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class DefaultStickerProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }
    
    [Required]
    public uint BasePanelId { get; set; } = 0;

    [Required] 
    public uint CommentPartsAId { get; set; } = 0;

    [Required] 
    public uint CommentPartsBId { get; set; } = 0;
    
    /// <summary>Maps to PlayerSticker.pilot_image_disp_flag (tag 6). true = show pilot image.</summary>
    [Required]
    public bool PilotImageDispFlag { get; set; } = false;

    /// <summary>Maps to PlayerSticker.cutin_image_use_flag (tag 8). true = use cutin as pilot image.</summary>
    [Required]
    public bool CutinImageUseFlag { get; set; } = false;

    [Required]
    public uint StickerBackgroundId { get; set; } = 0;

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