using MudBlazor;
using Squads.Shared.Users;
using Squads.Shared.PricePlans;
using Microsoft.AspNetCore.Components;

namespace Squads.Client.PricePlans.Components;

public partial class PricePlanList
{

    public List<PricePlanDto.Detail> Plans { get; set; }
    public List<UserItem> Users = new();
    public bool Loading = true;
    [Inject] private PricePlanState _state { get; set; }
    [Inject] private IUserService _userService { get; set; }
    [Inject] private IDialogService _dialogService { get; set; }
    string Alert { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await StartUp();
    }

    private async Task StartUp(){
        _state.OnChange += UpdateComponent;
        await GetUsers();
        Loading=false;
        StateHasChanged();
    }

    private void UpdateComponent()
    {
        Plans = _state.PricePlans;
        StateHasChanged();
    }

    private async Task GetUsers()
    {
        UserRequest.GetIndex req = new();
        var res = await _userService.GetIndexAsync(req);
        Users = res.Users.Select(
            u => new UserItem(u.Id, $"{u.FirstName} {u.LastName}", u.ActivePricePlan.Id)
        ).ToList();
    }

    public void UpdatePricePlan(PricePlanDto.Detail pricePlan)
    {
        DialogOptions options = new() {MaxWidth=MaxWidth.Medium, FullWidth=true, CloseButton=true};
        var parameters = new DialogParameters {["PricePlan"]=pricePlan};
        var dialog = _dialogService.Show<PricePlanEditDialog>("Prijs segment aanpassen.", parameters, options);
    }

    public void DeletePricePlan(PricePlanDto.Detail pricePlan)
    {
        if(int.Parse(AmountOfUsersInPricePlan(pricePlan.Id)) != 0)
        {
            Alert = "Kan geen prijs segment verwijderen waaraan nog klanten gelinkt zijn.";
        }   
        else
        {
            Alert = null;
            _state.DeletePricePlan(pricePlan);
        }
    }

    public string AmountOfUsersInPricePlan(int pricePlanId)
    {
        return Users.Where(u => u.PricePlanId == pricePlanId.ToString()).Count().ToString();
    }

    public async void ItemUpdated(MudItemDropInfo<UserItem> dropItem)
    {
        dropItem.Item.PricePlanId = dropItem.DropzoneIdentifier;
        await _userService.UpdatePricePlan(dropItem.Item.Id, int.Parse(dropItem.Item.PricePlanId));
    }

    public class UserItem
    {
        public int Id {get; set;}
        public string Name { get; set; }
        public string PricePlanId { get; set; }

        public UserItem(int id, string name, int pricePlanId)
        {
            Id = id;
            Name = name;
            PricePlanId = pricePlanId.ToString();
        }
    }
}
