using Microsoft.AspNetCore.Components;
using MudBlazor;
using Squads.Shared.PricePlans;
using FluentValidation;

namespace Squads.Client.PricePlans.Components;

public partial class PricePlanEditDialog
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public PricePlanDto.Detail PricePlan { get; set; }
    [Inject] PricePlanState _state { get; set; }
    MudForm Form;
    PricePlanValidator Validator = new();
    bool Success;
    string[] Errors = {};
    bool Loading = true;

    PricePlanDto.Mutate mutate = new();

    protected override async Task OnInitializedAsync()
    {
        if(PricePlan != null)
        {
            mutate.name = PricePlan.name;
            mutate.SubscriptionPrice = PricePlan.SubscriptionPrice;
            mutate.TurnPrice = PricePlan.TurnPrice;
        }
        Loading = false;
        StateHasChanged();
    }

    public async Task Submit()
    { 
        await Form.Validate();

        if(Form.IsValid)
        {
            Loading = true;
            StateHasChanged();
            await ProcessForm();
            Loading = false;
            MudDialog.Close();
        }
    }

    private async Task ProcessForm()
    {
        if(PricePlan == null)
        {
            await CreateNew();
        }
        else
        {
            await UpdateExisting();
        }
    }

    private async Task CreateNew()
    {
        await _state.AddPricePlan(mutate);
    }

    private async Task UpdateExisting()
    {
        _state.UpdatePricePlan(PricePlan.Id, mutate); 
    }

    public class PricePlanValidator : AbstractValidator<PricePlanDto.Mutate>
    {
        public PricePlanValidator()
        {
            RuleFor(p => p.name)
                .NotEmpty()
                .Length(1,100);

            RuleFor(p => p.SubscriptionPrice)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);

            RuleFor(p => p.SubscriptionPrice)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {   
            var result = await ValidateAsync(ValidationContext<PricePlanDto.Mutate>.CreateWithOptions((PricePlanDto.Mutate)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        }; 
    }
}