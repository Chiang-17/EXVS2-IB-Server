using WebUIInfinite.Shared.Dto.Enum;

namespace WebUIInfinite.Shared.Dto.Common;

public class BasicProfile
{
    public uint UserId { get; set; } = 0;
    public string UserName { get; set; } = string.Empty;
    public uint OpenRecord { get; set; } = 0;
    /// <summary>1 = open_player_level true; 0 = false. Legacy multi-option IDs normalized to 0/1 on load/save.</summary>
    public uint OpenEchelon { get; set; } = 0;
    public uint DefaultGaugeDesignId { get; set; } = 0;
    public BgmPlayingMethod DefaultBgmPlayingMethod { get; set; } = BgmPlayingMethod.Random;
    public uint[] DefaultBgmList { get; set; } = Array.Empty<uint>();
    public uint[] StageRandoms { get; set; } = Array.Empty<uint>();
    public Title DefaultTitle { get; set; } = new();
    // Nullable to distinguish an old client's omitted fields from an explicit clear.
    public Title? SubTitle1 { get; set; }
    public Title? SubTitle2 { get; set; }
    public Title TriadTitle { get; set; } = new();
    public Title ClassMatchTitle { get; set; } = new();
    public bool IsFixedRadar { get; set; } = false;
    public bool EnableQuickStart { get; set; } = false;
    public int MarkerColorId { get; set; } = 0;
    public uint Gp { get; set; } = 0;
    // Client-aligned new field (LoadCard.MobileUserGroup.CustomizeGroup tag 5).
    public bool BgmSafeMode { get; set; } = false;
}
