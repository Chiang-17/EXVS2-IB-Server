using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerInfinite.Constants;
using ServerInfinite.Models.Cards.Team;
using ServerInfinite.Persistence;

namespace ServerInfinite.Strategy.Team;

// Helper to assemble a TagTeamGroup matching the new client schema. Both append strategies
// (leader / partner) call into this so the required `tag_status` and the optional
// partner_* enrichment fields are populated consistently.
//
// Tags 13-19 are the client's new layout; tags 50-57 carry the legacy server-only
// styling/economy fields that are still owned by the WebUI editor.
internal static class TagTeamGroupBuilder
{
    public static TagTeamGroup Build(ServerDbContext context, TagTeamData tagTeam, int partnerCardId)
    {
        var group = new TagTeamGroup
        {
            Id = (uint)tagTeam.Id,
            Name = tagTeam.TeamName,
            PartnerId = (uint)partnerCardId,
            // Client schema marks `tag_status` as required. Server has no team-status concept
            // yet, so default to 0 ("active/idle") until a richer team lifecycle is modelled.
            TagStatus = 0,
            // Legacy server-only fields (tags 50-57), preserved for WebUI round-trip.
            SkillPoint = tagTeam.SkillPoint,
            SkillPointBoost = TeamConstants.TagSkillPointBoost,
            BackgroundPartsId = tagTeam.BackgroundPartsId,
            EffectId = tagTeam.EffectId,
            EmblemId = tagTeam.EmblemId,
            BgmId = tagTeam.BgmId,
            NameColorId = tagTeam.NameColorId,
            BoostRemains = TeamConstants.BoostRemains,
        };

        EnrichPartnerInfo(context, group, partnerCardId);
        return group;
    }

    static void EnrichPartnerInfo(ServerDbContext context, TagTeamGroup group, int partnerCardId)
    {
        var partner = context.CardProfiles
            .Include(x => x.PlayerLevel)
            .Include(x => x.TeamClassMatchRecord)
            .FirstOrDefault(x => x.Id == partnerCardId);

        if (partner is null)
        {
            return;
        }

        group.PartnerName = partner.UserName;

        if (partner.PlayerLevel is not null)
        {
            group.PartnerPlayerLevelId = partner.PlayerLevel.PlayerLevelId;
            group.PartnerBadgeId = partner.PlayerLevel.BadgeId;
        }

        if (partner.TeamClassMatchRecord is not null)
        {
            group.PartnerClassId = partner.TeamClassMatchRecord.ClassId;
            // Server has no per-team grade concept yet; leave PartnerGradeId at 0.
        }
    }
}
