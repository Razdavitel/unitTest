using Microsoft.AspNetCore.Components;
using MudBlazor;
using Squads.Client.PricePlans.Components;
using Squads.Shared.PricePlans;

namespace Squads.Client.PricePlans;

public partial class Index
{
    [Inject] private IDialogService _dialogService { get; set; }
    [Inject] private PricePlanState _state { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await _state.GetPricePlans();
    }

    public async void CreateNew()
    {   
        DialogOptions options = new() {MaxWidth=MaxWidth.Medium, FullWidth=true, CloseButton=true};
        var dialog = _dialogService.Show<PricePlanEditDialog>("Nieuwe prijs segment maken.", options);
        var res = await dialog.Result;
        if(!res.Cancelled)
        {
        }
    }
}