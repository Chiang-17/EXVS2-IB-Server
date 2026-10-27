using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerInfinite.Models.Cards.Battle;
using ServerInfinite.Models.Cards.Settings;

namespace ServerInfinite.Persistence.Configurations.Cards.Battle;

// Relationships are configured on the dependent side only, so CardProfile does not need new navigations.

public class StageSaveDataConfigurations : IEntityTypeConfiguration<StageSaveData>
{
    public void Configure(EntityTypeBuilder<StageSaveData> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.CardId).IsUnique();
        builder.HasOne(x => x.CardProfile).WithOne().HasForeignKey<StageSaveData>(x => x.CardId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}

public class RouteBattleHighScoreConfigurations : IEntityTypeConfiguration<RouteBattleHighScore>
{
    public void Configure(EntityTypeBuilder<RouteBattleHighScore> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => new { x.CardId, x.Difficulty }).IsUnique();
        builder.HasOne(x => x.CardProfile).WithMany().HasForeignKey(x => x.CardId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
