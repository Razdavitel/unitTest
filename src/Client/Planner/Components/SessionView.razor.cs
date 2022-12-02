using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Squads.Shared.Sessions;
using Squads.Shared.Users;
using Squads.Shared.Workouts;
namespace Squads.Client.Planner.Components
{
    public partial class SessionView
    {
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public ISessionService SessionService { get; set; }
        [Inject] public IWorkoutService WorkoutService { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Parameter]
        public SessionDto.Detail Session { get; set; }
        [Parameter]
        public WorkoutDto.Detail Workout { get; set; }

        protected override async Task OnInitializedAsync()
        {
            /*WorkoutRequest.GetDetail req2 = new WorkoutRequest.GetDetail { WorkoutId = Session.Workout.Id };
            var res2 = await WorkoutService.GetDetailAsync(req2);
            Workout = res2.Workout;*/

        }
        private async Task OnEditSessionAsync()
        {
            SessionDto.Mutate ses = await DialogService.OpenAsync<EditAppointmentPage>("Afspraak wijzigen", new Dictionary<string, object> { { "Session", Session } });

            SessionRequest.Edit request = new()
            {
                SessionId = Session.Id,
                Session = ses
            };
            var response = await SessionService.EditAsync(request);

            SessionRequest.GetDetail req1 = new SessionRequest.GetDetail { SessionId = response.SessionId };
            var res1 = await SessionService.GetDetailAsync(req1);
            var updatedSession = res1.Session;


            if (response.SessionId != 0)
            {
                var d = new AppointmentData
                {
                    Start = ses.StartsAt,
                    End = ses.EndsAt,
                    Text = ses.Title,
                    Data = new SessionDto.Index
                    {
                        Id = updatedSession.Id,
                        Title = updatedSession.Title,
                        //Description = ses.Description,
                        StartsAt = updatedSession.StartsAt,
                        EndsAt = updatedSession.EndsAt,
                        Workout = updatedSession.Workout,
                        Trainees= updatedSession.Trainees,
                        
                        // Coach= new UserDto.Index { Id=ures.User.Id }

                    }
                };
            }
            DialogService.Close(Session);

        }

        private async Task OnDeleteSessionAsync()
        {
            var request = new SessionRequest.Delete { SessionId = Session.Id };
            await SessionService.DeleteAsync(request);
            DialogService.Close();

        }
    }
}