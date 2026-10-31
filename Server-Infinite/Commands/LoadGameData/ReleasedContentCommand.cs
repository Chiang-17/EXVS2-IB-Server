using nue.protocol.exvs;
using ServerInfinite.Models.Config;

namespace ServerInfinite.Commands.LoadGameData;

public class ReleasedContentCommand : ILoadGameDataCommand
{
    private readonly CardServerConfig _config;

    public ReleasedContentCommand(CardServerConfig config)
    {
        _config = config;
    }
    
    public void Fill(Response.LoadGameData loadGameData)
    {
        var releasedMobileSuits = Enumerable.Range(1, 400).Select(i => (uint)i).ToArray();
        var releasedNavis = Enumerable.Range(1, 100).Select(i => (uint)i).ToArray();
        // Default version markers follow update_and_new_ms.txt from the IB v13 resource pack.
        var newMobileSuits = _config.GameConfigurations.NewMobileSuits ?? new[] { 330u, 331u, 332u, 333u, 334u, 335u };
        var updatedMobileSuits = _config.GameConfigurations.UpdatedMobileSuits ?? [
            19u, 250u, 142u, 64u, 112u, 164u, 108u, 259u, 90u, 146u,
            265u, 252u, 191u, 327u, 318u, 140u, 269u, 325u, 288u, 290u,
            294u, 224u, 97u, 242u, 5u, 330u, 279u, 170u, 165u, 41u,
            156u, 37u, 127u, 308u, 293u, 79u, 253u, 168u, 166u, 301u,
            292u, 21u, 8u, 195u, 176u, 129u, 270u, 91u, 289u, 326u,
            331u, 152u, 187u, 207u, 300u, 216u, 205u, 98u
        ];

        loadGameData.ReleaseMsIds = releasedMobileSuits;
        loadGameData.DisplayableMsIds = releasedMobileSuits;
        loadGameData.ReleaseGuestNavIds = releasedNavis;
        loadGameData.NewMsIds = newMobileSuits;
        loadGameData.UpdateMsIds = updatedMobileSuits;
        loadGameData.MobileSuitBlocklists = new[] { 5u, 39u, 58u, 198u, 214u, 215u, 218u, 241u, 256u, 288u };
    }
}
