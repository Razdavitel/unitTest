using Microsoft.AspNetCore.Components;
using MudBlazor;
using Squads.Shared.Users;

namespace Squads.Client.Users.Components;

public partial class UserTable
{
    private int selectedRowNumber = -1;
    private MudTable<UserDto.Index>? mudTable;
    private List<string> clickedEvents = new();
    private string searchString = "";
    private List<UserDto.Index> customers = new List<UserDto.Index>();


    [Inject] public IUserService UserService { get; set; }
    [Inject] public ISnackbar SnackBar { get; set; }
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

    private bool Search(UserDto.Index customer)
    {
        if (string.IsNullOrWhiteSpace(searchString)) return true;
        return customer.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            || customer.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase);
    }

    private void RowClickEvent(TableRowClickEventArgs<UserDto.Index> tableRowClickEventArgs)
    {
        clickedEvents.Add("Row has been clicked");
        Edit(tableRowClickEventArgs.Item);
    }

    private string SelectedRowClassFunc(UserDto.Index element, int rowNumber)
    {
        if (selectedRowNumber == rowNumber)
        {
            selectedRowNumber = -1;
            clickedEvents.Add("Selected Row: None");
            return string.Empty;
        }
        else if (mudTable!.SelectedItem != null && mudTable.SelectedItem.Equals(element))
        {
            selectedRowNumber = rowNumber;
            clickedEvents.Add($"Selected Row: {rowNumber}");
            return "selected";
        }
        else
        {
            return string.Empty;
        }
    }

    private async Task Edit(UserDto.Index user)
    {
        var req = new UserRequest.GetDetail() { UserId = user.Id };
        var res = await UserService.GetDetailAsync(req);
        var parameters = new DialogParameters { ["user"] = res.User };

        var dialog = DialogService.Show<EditUserForm>("Edit User", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
        }

        await GetCustomers();

    }


    private async Task Delete(UserDto.Index user)
    {
        var parameters = new DialogParameters { ["user"] = user };

        var dialog = DialogService.Show<DeleteForm>("Delete User", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
        }

        await GetCustomers();
    }
}
