using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerInfinite.Models.Cards.Battle;

/// <summary>
/// Route battle best records per difficulty
/// (SaveStageResult -> LoadCard.pilot_data_group.high_score_info).
/// ClearTime: smaller is better (0 = no record). ClearScore: larger is better.
/// </summary>
[Table("exvs2ob_route_battle_high_score")]
[Index(nameof(Id))]
public class RouteBattleHighScore : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int CardId { get; set; }

    [Required]
    public uint Difficulty { get; set; }

    [Required]
    public uint ClearTime1 { get; set; }

    [Required]
    public uint ClearScore1 { get; set; }

    [Required]
    public uint ClearTime2 { get; set; }

    [Required]
    public uint ClearScore2 { get; set; }

    [Required]
    public uint ClearTime3 { get; set; }

    [Required]
    public uint ClearScore3 { get; set; }

    [Required]
    public uint TotalClearTime { get; set; }

    [Required]
    public uint TotalClearScore { get; set; }

    public virtual CardProfile CardProfile { get; set; } = null!;
}
