using System.Net.Http.Json;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Throw;
using WebUIInfinite.Client.Context.CustomizeCard;
using WebUIInfinite.Client.Services;
using WebUIInfinite.Client.Validator;
using WebUIInfinite.Shared.Dto.Common;
using WebUIInfinite.Shared.Dto.Request.Sticker;
using WebUIInfinite.Shared.Dto.Response;
using WebUIInfinite.Shared.Resources;

namespace WebUIInfinite.Client.Command.CustomizeCard.Save;

public class MobileSuitStickerSettingsSaver : ICustomizeCardContentSaver
{
    private readonly HttpClient _httpClient;
    private readonly INameValidator _nameValidator;
    private readonly IResponseSnackService _responseSnackService;
    private readonly IStringLocalizer<Resource> _localizer;

    public MobileSuitStickerSettingsSaver(HttpClient httpClient, INameValidator nameValidator, IResponseSnackService responseSnackService, IStringLocalizer<Resource> localizer)
    {
        _httpClient = httpClient;
        _nameValidator = nameValidator;
        _responseSnackService = responseSnackService;
        _localizer = localizer;
    }

    public async Task Save(CustomizeCardContext customizeCardContext, ProgressContext progressContext, ISnackbar snackbar, Action stateHasChanged)
    {
        progressContext.HideMobileSuitStickerProgress = "visible";
        stateHasChanged.Invoke();

        foreach (var sticker in customizeCardContext.MobileSuitStickerSettings)
        {
            StickerPilotPoolDefaults.ApplyEmptyPoolDefaultsOnSave(sticker);
        }

        var dto = new UpsertMobileSuitStickersRequest()
        {
            AccessCode = customizeCardContext.AccessCode,
            ChipId = customizeCardContext.ChipId,
            MsStickerList = customizeCardContext.MobileSuitStickerSettings
        };
        
        var response = await _httpClient.PostAsJsonAsync("/ui/sticker/upsertMobileSuitStickers", dto);
        var result = await response.Content.ReadFromJsonAsync<BasicResponse>();
        result.ThrowIfNull();
        
        _responseSnackService.ShowBasicResponseSnack(snackbar, result, _localizer["save_hint_ms_sticker"]);

        progressContext.HideMobileSuitStickerProgress = "invisible";
        stateHasChanged.Invoke();
    }
}