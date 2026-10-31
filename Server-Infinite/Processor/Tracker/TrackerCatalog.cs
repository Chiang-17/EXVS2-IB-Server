using System.Text.Json;
using Microsoft.Extensions.FileProviders;
using WebUIInfinite.Shared.Dto.Common;

namespace ServerInfinite.Processor.Tracker;

public interface ITrackerCatalog
{
    IReadOnlyDictionary<uint, TrackerType> Load();
    IReadOnlyList<Familiarity> LoadMobileSuitFamiliarities();
}

// The same external JSON is served to WebUI. Take one snapshot per LoadCard so
// resource updates become effective on the next card read without recompiling.
public sealed class TrackerCatalog(IFileProvider files) : ITrackerCatalog
{
    public const string ResourcePath = "data/sticker/TrackerTypes.json";
    public const string MasteryResourcePath = "data/mastery/MobileSuitFamiliarity.json";

    public IReadOnlyDictionary<uint, TrackerType> Load()
    {
        using var stream = files.GetFileInfo(ResourcePath).CreateReadStream();
        var items = JsonSerializer.Deserialize<List<TrackerType>>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? throw new InvalidDataException("Tracker catalog is null");
        return items.ToDictionary(x => x.Id);
    }

    public IReadOnlyList<Familiarity> LoadMobileSuitFamiliarities()
    {
        using var stream = files.GetFileInfo(MasteryResourcePath).CreateReadStream();
        return JsonSerializer.Deserialize<List<Familiarity>>(stream, new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? throw new InvalidDataException("Mobile suit familiarity catalog is null");
    }

    // Used by standalone probes; normal hosting injects the static-web-assets provider.
    public static ITrackerCatalog FromOutput() => new TrackerCatalog(
        new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "wwwroot")));
}
