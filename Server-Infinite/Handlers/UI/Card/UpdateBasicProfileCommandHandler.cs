using MediatR;
using Microsoft.EntityFrameworkCore;
using ServerInfinite.Models.Cards.Titles.User;
using ServerInfinite.Persistence;
using WebUIInfinite.Shared.Dto.Common;
using WebUIInfinite.Shared.Dto.Request;
using WebUIInfinite.Shared.Dto.Response;
using WebUIInfinite.Shared.Exception;
using WebUIInfinite.Shared.Validation;

namespace ServerInfinite.Handlers.UI.Card;

public record UpdateBasicProfileCommand(UpdateBasicProfileRequest Request) : IRequest<BasicResponse>;

public class UpdateBasicProfileCommandHandler : IRequestHandler<UpdateBasicProfileCommand, BasicResponse>
{
    private readonly ServerDbContext _context;

    public UpdateBasicProfileCommandHandler(ServerDbContext context)
    {
        _context = context;
    }

    public Task<BasicResponse> Handle(UpdateBasicProfileCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        if (updateRequest.BasicProfile == null)
        {
            throw new InvalidRequestDataException("Basic Profile is null");
        }

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile == null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }
        
        var basicProfile = updateRequest.BasicProfile;

        if (!PlayerTextPolicy.IsValid(basicProfile.UserName, required: true))
            throw new InvalidRequestDataException("Player name is empty or contains invalid characters");
        if (new[] { basicProfile.DefaultTitle, basicProfile.SubTitle1, basicProfile.SubTitle2,
                basicProfile.TriadTitle, basicProfile.ClassMatchTitle }
            .Any(title => title is not null && !PlayerTextPolicy.IsValid(title.CustomText, required: false)))
            throw new InvalidRequestDataException("Custom title contains invalid characters");

        if (basicProfile.MarkerColorId < 0)
            throw new InvalidRequestDataException("Marker color ID must be non-negative");
        
        var playerProfile = _context.PlayerProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var userTitleSetting = _context.UserTitleSettingDbSet
            .Include(x => x.UserDefaultTitle)
            .Include(x => x.UserSubTitle1)
            .Include(x => x.UserSubTitle2)
            .Include(x => x.UserTriadTitle)
            .Include(x => x.UserClassMatchTitle)
            .First(x => x.CardProfile == cardProfile);
        
        var customizeProfile = _context.CustomizeProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        var generalSetting = _context.GeneralSettingDbSet
            .First(x => x.CardProfile == cardProfile);

        cardProfile.UserName = basicProfile.UserName;
        playerProfile.OpenEchelon = basicProfile.OpenEchelon != 0 ? 1u : 0u;
        playerProfile.OpenRecord = basicProfile.OpenRecord;

        UpdateTitle(userTitleSetting.UserDefaultTitle, basicProfile.DefaultTitle);
        if (basicProfile.SubTitle1 is not null)
            UpdateTitle(userTitleSetting.UserSubTitle1, basicProfile.SubTitle1);
        if (basicProfile.SubTitle2 is not null)
            UpdateTitle(userTitleSetting.UserSubTitle2, basicProfile.SubTitle2);
        UpdateTitle(userTitleSetting.UserTriadTitle, basicProfile.TriadTitle);
        UpdateTitle(userTitleSetting.UserClassMatchTitle, basicProfile.ClassMatchTitle);

        customizeProfile.DefaultGaugeDesignId = basicProfile.DefaultGaugeDesignId;
        customizeProfile.DefaultBgmPlayMethod = (uint) basicProfile.DefaultBgmPlayingMethod;
        customizeProfile.DefaultBgmSettings = String.Join(",", basicProfile.DefaultBgmList.ToArray());
        customizeProfile.StageRandoms = String.Join(",", basicProfile.StageRandoms.ToArray());
        customizeProfile.BgmSafeMode = basicProfile.BgmSafeMode;

        generalSetting.FixPositionRadar = basicProfile.IsFixedRadar;
        generalSetting.EnableQuickStart = basicProfile.EnableQuickStart;
        generalSetting.MarkerColorId = basicProfile.MarkerColorId;
        
        _context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
    
    void UpdateTitle(UserTitle userTitle, Title title)
    {
        userTitle.TitleTextId = title.TextId;
        userTitle.TitleOrnamentId = title.OrnamentId;
        userTitle.TitleEffectId = title.EffectId;
        userTitle.TitleBackgroundPartsId = title.BackgroundPartsId;
        userTitle.CustomText = title.CustomText;
    }
}
