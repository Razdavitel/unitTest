using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Squads.Client.Shared
{

    public partial class Alert
    {
        [Parameter]
        public DialogParameters Parameters { get; set; }
        private string message="";

        private Severity severity;

        protected override async Task OnInitializedAsync()
        {
            message = Parameters.Get<string>("ContentText");
            severity = Parameters.Get<Severity>("Severity");
        }
    }
}