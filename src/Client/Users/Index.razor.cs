using Microsoft.AspNetCore.Components;
using MudBlazor;
using Squads.Client.Users.Components;
using Squads.Shared.Users;

namespace Squads.Client.Users;

public partial class Index
{
    private List<UserDto.Index> customers = new List<UserDto.Index>();
    [Inject] public IUserService UserService { get; set; }
    [Inject] public IDialogService DialogService { get; set; }



    protected override async Task OnInitializedAsync()
    {
        await GetCustomers();
    }

    private async Task GetCustomers()
    {
        var req = new UserRequest.GetIndex();
        var res = await UserService.GetIndexAsync(req);
        customers = res.Users;
    }

    private async Task Create()
    {
        DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true};
        var dialog = DialogService.Show<CreateForm>("Create User", options);
        var result = await dialog.Result;
    
        if (!result.Cancelled)
        {
        }

        await GetCustomers();
    }


}
