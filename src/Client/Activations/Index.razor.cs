using Microsoft.AspNetCore.Components;

namespace Squads.Client.Activations;

public partial class Index
{
    [Parameter]
    [SupplyParameterFromQuery]
    public string Token { get; set; }
}