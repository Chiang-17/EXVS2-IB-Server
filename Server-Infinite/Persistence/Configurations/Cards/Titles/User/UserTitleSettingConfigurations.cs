using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerInfinite.Models.Cards.Titles.User;

namespace ServerInfinite.Persistence.Configurations.Cards.Titles.User;

public class UserTitleSettingConfigurations : IEntityTypeConfiguration<UserTitleSetting>
{
    public void Configure(EntityTypeBuilder<UserTitleSetting> builder)
    {
        builder.HasKey(x => x.UserTitleSettingId);
        
        builder.HasOne(e => e.UserDefaultTitle)
            .WithOne(e => e.UserTitleSetting)
            .HasForeignKey<UserDefaultTitle>(e => e.UserTitleSettingId)
            .IsRequired();

        builder.HasOne(e => e.UserSubTitle1)
            .WithOne(e => e.UserTitleSetting)
            .HasForeignKey<UserSubTitle1>(e => e.UserTitleSettingId)
            .IsRequired();

        builder.HasOne(e => e.UserSubTitle2)
            .WithOne(e => e.UserTitleSetting)
            .HasForeignKey<UserSubTitle2>(e => e.UserTitleSettingId)
            .IsRequired();
        
        builder.HasOne(e => e.UserTriadTitle)
            .WithOne(e => e.UserTitleSetting)
            .HasForeignKey<UserTriadTitle>(e => e.UserTitleSettingId)
            .IsRequired();
        
        builder.HasOne(e => e.UserClassMatchTitle)
            .WithOne(e => e.UserTitleSetting)
            .HasForeignKey<UserClassMatchTitle>(e => e.UserTitleSettingId)
            .IsRequired();
    }
}
