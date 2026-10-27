using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;

namespace ServerInfinite.Commands.LoadCard.MobileUser;

/// <summary>LoadCard.mobile_user_group.save_data_info (tag 35): route battle continue data.</summary>
public class LoadStageSaveDataCommand : ILoadCardMobileUserCommand
{
    private readonly ServerDbContext _context;

    public LoadStageSaveDataCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var cardId = cardProfile.Id;
        var data = _context.StageSaveDataDbSet.FirstOrDefault(x => x.CardId == cardId);
        if (data == null)
        {
            return;
        }

        var info = new Response.LoadCard.MobileUserGroup.SaveDataInfo
        {
            PatternId = data.PatternId,
            Difficulty = data.Difficulty,
            StageNum = data.StageNum,
            BossId1 = data.BossId1,
            MaxGauge = data.MaxGauge,
            CurrentGauge = data.CurrentGauge,
            MstMobileSuitId = data.MstMobileSuitId,
            CustomChipIds = ParseIds(data.CustomChipIds),
            SaveDataUrl = data.SaveDataUrl ?? string.Empty
        };
        if (data.BossId2.HasValue)
        {
            info.BossId2 = data.BossId2.Value;
        }

        mobileUserGroup.save_data_info = info;
    }

    internal static uint[] ParseIds(string? s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return Array.Empty<uint>();
        }

        return s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(v => uint.TryParse(v, out var n) ? (uint?)n : null)
            .Where(v => v.HasValue)
            .Select(v => v!.Value)
            .ToArray();
    }
}
