using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Throw;
using WebUIInfinite.Shared.Dto.Common;
using WebUIInfinite.Shared.Dto.Rank;

namespace WebUIInfinite.Client.Pages;

public partial class ServerPlayerLevelRanks
{
    [Inject]
    private IJSRuntime? _jsRuntime { get; set; }

    private readonly List<BreadcrumbItem> breadcrumbs = new();
    
    private string? errorMessage = null;
    private bool _loading = true;
    private PlayerLevelRankData _playerLevelRankData = new();
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        breadcrumbs.Add(new BreadcrumbItem(localizer["server_player_level_ranks"], href: "/ServerPlayerLevelRanks", disabled: false));
        
        var playerLevelRankData = await Http.GetFromJsonAsync<PlayerLevelRankData>("/ui/rank/player-level-rank/getPlayerLevelRanks");
        playerLevelRankData.ThrowIfNull();

        _playerLevelRankData = playerLevelRankData;
    }
    
    protected override void OnParametersSet()
    {
        if (_playerLevelRankData == new PlayerLevelRankData())
        {
            _loading = true;
            return;
        }

        _loading = false;
    }
    
}
