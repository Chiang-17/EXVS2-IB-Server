namespace WebUIInfinite.Shared.Dto.Player;

public class PlayerLevelProfile
{
    public uint RoundLevel { get; set; } = 0;
    public uint PlayerLevel { get; set; } = 0;
    public uint CurrentExp { get; set; } = 0;
    public bool AbleToStepUpRound { get; set; } = false;
    public ExpRequirement ExpRequirement { get; set; } = new();
    /// <summary>Route battle badge id (vsac29). Shown as images/Badge/{BadgeId}.webp (or .png).</summary>
    public uint BadgeId { get; set; } = 0;
    public int BadgeExp { get; set; } = 0;
}