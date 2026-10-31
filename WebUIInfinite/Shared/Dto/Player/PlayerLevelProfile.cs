namespace WebUIInfinite.Shared.Dto.Player;

public class PlayerLevelProfile
{
    // Retained for older API clients; current server always returns zero/false.
    public uint RoundLevel { get; set; } = 0;
    public uint PlayerLevel { get; set; } = 0;
    public uint CurrentExp { get; set; } = 0;
    public bool AbleToStepUpRound { get; set; } = false;
    public ExpRequirement ExpRequirement { get; set; } = new();
}
