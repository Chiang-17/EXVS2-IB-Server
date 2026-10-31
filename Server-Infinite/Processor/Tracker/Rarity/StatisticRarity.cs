namespace ServerInfinite.Processor.Tracker.Rarity;

public static class StatisticRarity
{
    public static uint Calculate(double value, IReadOnlyList<double>? thresholds)
    {
        if (!double.IsFinite(value) || value < 0 || thresholds is null) return 0;
        for (var i = thresholds.Count - 1; i >= 0; i--)
            if (value >= thresholds[i]) return (uint)i;
        return 0;
    }

}
