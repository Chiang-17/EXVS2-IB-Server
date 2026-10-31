using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerInfinite.Models.Cards.Settings;

/// <summary>
/// Route battle "continue" save data (SaveStageResult.result.save_data_info -> LoadCard.mobile_user_group.save_data_info).
/// Maps to the already existing table created by migration 20260529190428_AddStageSaveData.
/// </summary>
[Table("exvs2ob_stage_save_data")]
[Index(nameof(Id))]
public class StageSaveData : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int CardId { get; set; }

    [Required]
    public uint PatternId { get; set; }

    [Required]
    public uint Difficulty { get; set; }

    [Required]
    public uint StageNum { get; set; }

    [Required]
    public uint BossId1 { get; set; }

    public uint? BossId2 { get; set; }

    [Required]
    public uint MaxGauge { get; set; }

    [Required]
    public uint CurrentGauge { get; set; }

    [Required]
    public uint MstMobileSuitId { get; set; }

    /// <summary>Comma separated uint list.</summary>
    [Required]
    public string CustomChipIds { get; set; } = string.Empty;

    [Required]
    public string SaveDataUrl { get; set; } = string.Empty;

    public virtual CardProfile CardProfile { get; set; } = null!;
}
