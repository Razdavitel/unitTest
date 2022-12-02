using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Squads.Client.Sessions.Components;
using Squads.Shared.Auth;
using Squads.Shared.Sessions;
using MudBlazor;

namespace Squads.Client.Sessions;

public partial class Index

{

    [Inject] public ISessionService SessionService { get; set; }
    private List<SessionDto.Index> sessions;
    [Inject] public Radzen.DialogService DialogService { get; set; }
    [Inject] public MudBlazor.DialogService MudDialogService { get; set; }
    RadzenScheduler<AppointmentData> scheduler = new();
    Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();
    IList<AppointmentData> data = new List<AppointmentData>();
    [Inject] public IAuthService AuthService { get; set; }
    private bool stateHasChanged = false;
    private DialogParameters parameters;

    protected override async Task OnInitializedAsync()
    {

        SessionRequest.GetIndex request = new();
        var response = await SessionService.GetIndexAsync(request);
        sessions = response.Sessions;

        foreach (SessionDto.Index session in sessions)
        {
            var d = new AppointmentData
            {
                Start = session.StartsAt,
                End = session.EndsAt,
                Text = session.Title,
                Data = session
            };
            data.Add(d);

        }

    }
    protected async Task UpdateScheduler()
    {
        data.Clear();
        SessionRequest.GetIndex request = new();
        var response = await SessionService.GetIndexAsync(request);
        sessions = response.Sessions;

        foreach (SessionDto.Index session in sessions)
        {
            var d = new AppointmentData
            {
                Start = session.StartsAt,
                End = session.EndsAt,
                Text = session.Title,
                Data = session
            };
            data.Add(d);

        }
        await scheduler.Reload();

    }
    void OnSlotRender(SchedulerSlotRenderEventArgs args)
    {

        // Highlight today in month view
        if (args.View.Text == "Month" && args.Start == DateTime.Today)
        {
            args.Attributes["style"] = "background: rgba(244,244,244,1);";
        }
        
    }

    async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<AppointmentData> args)
    {
        stateHasChanged = false;
        SessionDto.Detail sesDet;
        SessionDto.Index fetchId = (SessionDto.Index)args.Data.Data;
        var id = fetchId.Id;
        // Fetch the latest version of the product before editing.
        var getResponse = await SessionService.GetDetailAsync(new SessionRequest.GetDetail { SessionId = id });
        sesDet = getResponse.Session;
        parameters = await DialogService.OpenAsync<SessionView>("Sessie", new Dictionary<string, object> { { "Session", sesDet } });
        
        stateHasChanged = true;

        await UpdateScheduler();
       
    }

    void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<AppointmentData> args)
    {
        // Never call StateHasChanged in AppointmentRender - would lead to infinite loop
        SessionDto.Index session = (SessionDto.Index)args.Data.Data;
        if (session.Trainees.Count == 6) {
            args.Attributes["style"] = "background: rgba(69,49,49,.8);";
        }
        else
        {
            args.Attributes["style"] = "background: rgba(64,66,66,0.8);";
        }

    }
}