using nue.protocol.exvs;
using ServerInfinite.Models.Cards.Settings;

namespace ServerInfinite.Mapper.Card.QuickStart;

public static class QuickStartInfoMapper
{
    public static void ApplyFromProto(
        QuickStartSetting setting,
        QuickStartInfo quickStartInfo)
    {
        setting.GameMode = quickStartInfo.GameMode;
        setting.RuleType = quickStartInfo.ShouldSerializeRuleType() ? quickStartInfo.RuleType : null;
        setting.TeamType = quickStartInfo.TeamType;
        setting.MstMobileSuitId0 = quickStartInfo.MstMobileSuitId0;
        setting.BurstType0 = quickStartInfo.BurstType0;
        // Save requests may omit the second suit; preserve the stored choice.
        if (quickStartInfo.ShouldSerializeMstMobileSuitId1())
            setting.MstMobileSuitId1 = quickStartInfo.MstMobileSuitId1;
        if (quickStartInfo.ShouldSerializeBurstType1())
            setting.BurstType1 = quickStartInfo.BurstType1;
        setting.StageId = quickStartInfo.StageId;
    }

    public static Response.LoadCard.MobileUserGroup.QuickStartInfo ToProto(QuickStartSetting setting)
    {
        var info = new Response.LoadCard.MobileUserGroup.QuickStartInfo
        {
            GameMode = setting.GameMode,
            TeamType = setting.TeamType,
            MstMobileSuitId0 = setting.MstMobileSuitId0,
            BurstType0 = setting.BurstType0,
            MstMobileSuitId1 = setting.MstMobileSuitId1,
            BurstType1 = setting.BurstType1,
            StageId = setting.StageId,
        };

        if (setting.RuleType is not null)
            info.RuleType = setting.RuleType.Value;

        return info;
    }
}
