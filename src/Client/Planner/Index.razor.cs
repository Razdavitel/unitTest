using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Squads.Client.Planner.Components;
using Squads.Shared.Auth;
using Squads.Shared.Sessions;
using Squads.Shared.Users;
using Squads.Shared.Workouts;

namespace Squads.Client.Planner
{
    public partial class Index
    {
        [Inject] public ISessionService SessionService { get; set; }
        [Inject] public IWorkoutService WorkoutService { get; set; }
        [Inject] public IUserService UserService { get; set; }
        private List<SessionDto.Index> sessions;
        [Inject] public DialogService DialogService { get; set; }
        RadzenScheduler<AppointmentData> scheduler = new();
        IList<AppointmentData> data = new List<AppointmentData>();
        [Inject] public IAuthService AuthService { get; set; }
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

            // Highlight working hours (9-18)
            /*           if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour > 8 && args.Start.Hour < 19)
                       {
                           args.Attributes["style"] = "background: rgba(255,220,40,.2);";
                       }*/
        }
        async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            SessionDto.Mutate ses = await DialogService.OpenAsync<AddAppointmentPage>("Afspraak toevoegen",
                     new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } });
            SessionRequest.Create request = new()
            {
                Session = ses
            };
            var response = await SessionService.CreateAsync(request);
            await UpdateScheduler();
        }

        async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<AppointmentData> args)
        {


            SessionDto.Index fetchId = (SessionDto.Index)args.Data.Data;
            var id = fetchId.Id;
            SessionRequest.GetDetail req = new() { SessionId = id };
            var getResponse = await SessionService.GetDetailAsync(req);

            var ses = await DialogService.OpenAsync<SessionView>("Sessie", new Dictionary<string, object> { { "Session", getResponse.Session } });
            await UpdateScheduler();
        }
        void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<AppointmentData> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop
            SessionDto.Index session = (SessionDto.Index)args.Data.Data;
            if (session.Trainees.Count == 6)
            {
                args.Attributes["style"] = "background: rgba(69,49,49,.8);";
            }
            else
            {
                args.Attributes["style"] = "background: rgba(64,66,66,0.8);";
            }
        }

    }
}