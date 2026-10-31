using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerInfinite.Models.Cards.Battle;

[Table("exvs2ob_battle_player_level")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class PlayerLevel : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint PlayerLevelId { get; set; } = 1;

    [Required]
    // Legacy database field; the current client has no player-level rounds.
    // Keep existing data, but never add this value to the protocol level.
    public uint PrestigeId { get; set; } = 0;

    [Required]
    public uint PlayerExp { get; set; } = 0;

    // The current client's badge is independent of player level and legacy prestige.
    [Required]
    public uint BadgeId { get; set; }

    [Required]
    public int BadgeExp { get; set; }

    [Required] 
    // Legacy flag; current WebUI and protocol do not offer round transitions.
    public bool LevelMaxDispFlag { get; set; } = false;
    
    public virtual CardProfile CardProfile { get; set; } = null!;
}
