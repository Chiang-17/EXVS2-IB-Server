using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Throw;
using WebUIInfinite.Client.Context.CustomizeCard;
using WebUIInfinite.Client.Services;
using WebUIInfinite.Client.Validator;
using WebUIInfinite.Shared.Dto.Request;
using WebUIInfinite.Shared.Dto.Response;
using WebUIInfinite.Shared.Resources;

namespace WebUIInfinite.Client.Command.CustomizeCard.Save;

public class BasicProfileSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly INameValidator _nameValidator;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public BasicProfileSaver(HttpClient httpClient, INameValidator nameValidator, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _nameValidator = nameValidator;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar, Action stateHasChanged)
    {
        if (_nameValidator.ValidatePlayerName(customizeCardContext.BasicProfile.UserName) is not null)
        {
            _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_cardinfo"]);
            return;
        }
        
        if (new[] { customizeCardContext.BasicProfile.DefaultTitle, customizeCardContext.BasicProfile.SubTitle1,
                customizeCardContext.BasicProfile.SubTitle2, customizeCardContext.BasicProfile.TriadTitle,
                customizeCardContext.BasicProfile.ClassMatchTitle }
            .Any(title => title is not null && _nameValidator.ValidateCustomizeTitle(title.CustomText) is not null))
        {
            _responseSnackService.ShowBasicResponseSnack(snackbar, new BasicResponse { Success = false }, _localizer["save_hint_cardinfo"]);
            return;
        }
        
        progressContext.HideProfileProgress = "visible";
        stateHasChanged.Invoke();
        
        var dto = new UpdateBasicProfileRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            BasicProfile = customizeCardContext.BasicProfile
        };
        
        var response = await _httpClient.PostAsJsonAsync("/ui/card/updateBasicProfile", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();
        
        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_cardinfo"]);

        progressContext.HideProfileProgress = "invisible";
        stateHasChanged.Invoke();
    }
}
