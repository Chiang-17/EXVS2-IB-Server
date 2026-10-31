using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerInfinite.Models.Cards.Settings;
using ServerInfinite.Models.Cards.Battle;

namespace ServerInfinite.Persistence.Configurations.Cards.Battle;

public class StageSaveDataConfigurations : IEntityTypeConfiguration<StageSaveData>
{
    public void Configure(EntityTypeBuilder<StageSaveData> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.CardId).IsUnique(true);
        builder.HasOne(x => x.CardProfile).WithOne().HasForeignKey<StageSaveData>(x => x.CardId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
    }
}

public class RouteBattleHighScoreConfigurations : IEntityTypeConfiguration<RouteBattleHighScore>
{
    public void Configure(EntityTypeBuilder<RouteBattleHighScore> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.CardId, x.Difficulty }).IsUnique(true);
        builder.HasOne(x => x.CardProfile).WithMany().HasForeignKey(x => x.CardId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
    }
}
