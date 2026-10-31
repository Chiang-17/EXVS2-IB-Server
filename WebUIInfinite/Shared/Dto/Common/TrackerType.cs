namespace WebUIInfinite.Shared.Dto.Common;

public class TrackerType : IdValuePair
{
    // External protobuf Tracker.type; passed through without HUD type conversion.
    public uint Type { get; set; }
    // Each index is a rarity; its value is the minimum level/mastery rank.
    public double[]? Threshold { get; set; }
}
