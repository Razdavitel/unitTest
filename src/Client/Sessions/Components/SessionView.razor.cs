using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radzen;
using Services.Users;
using Squads.Client.Shared;
using Squads.Shared.Sessions;
using Squads.Shared.Users;
using Squads.Shared.Workouts;
namespace Squads.Client.Sessions.Components
{
    public partial class SessionView
    {
        [Inject] public Radzen.DialogService DialogService { get; set; }
        [Inject] public ISessionService SessionService { get; set; }
        [Inject] public IWorkoutService WorkoutService { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Inject] public AuthenticationStateProvider GetAuthenticationStateAsync { get; set; }
        [Inject] public MudBlazor.DialogService MudDialogService { get; set; }
        [Parameter] public SessionDto.Detail Session { get; set; }
        [Parameter] public WorkoutDto.Detail Workout { get; set; }
        private UserDto.Detail user;
        private bool hasJoined = false;
        private bool isFull = false;


        protected override async Task OnInitializedAsync()
        {
            WorkoutRequest.GetDetail req = new WorkoutRequest.GetDetail { WorkoutId = Session.Id };
            var res = await WorkoutService.GetDetailAsync(req);
            Workout = res.Workout;

            var authstate = await GetAuthenticationStateAsync.GetAuthenticationStateAsync();
            var userEmail = authstate.User.Identity.Name;

            Console.WriteLine(userEmail);
            UserRequest.GetDetail userReq = new UserRequest.GetDetail { Email = userEmail };
            var userResult = await UserService.GetDetailByEmailAsync(userReq);
            var userDetail = userResult.User;
            user = userDetail;
            hasJoined = Session.Trainees.Exists(s => s.Id == user.Id);
            isFull = Session.Trainees.Count >= 6;
        }
        private async Task OnJoinSessionAsync()

        {

            if (!isFull)
            {
                var ses = new SessionDto.MutateTrainees
                {
                    TraineeId = user.Id
                };

                SessionRequest.MutateTrainee req = new SessionRequest.MutateTrainee
                {
                    SessionId = Session.Id,
                    MutateTrainees = ses

                };
                var res = await SessionService.AddTrainee(req);
                Session.Trainees?.Add(user);
                hasJoined = true;
                if (res != null)
                {
                    string message = "U bent succesvol toegevoegd aan de training!";
                    var param = new DialogParameters();
                    param.Add("Severity", Severity.Success);
                    param.Add("ContentText", message);
                    DialogService.Close(param);
                }
                else
                {
                    string message = "Er is een fout opgetreden tijdens het proberen deel te nemen aan een training!";
                    DialogParameters param = new DialogParameters();
                    param.Add("Severity", Severity.Success);
                    param.Add("ContentText", message);
                    DialogService.Close(param);


                }
                isFull = Session.Trainees?.Count >= 6;
            }
        }
        private async Task OnLeaveSessionAsync()
        {
            var ses = new SessionDto.MutateTrainees
            {
                TraineeId = user.Id
            };

            SessionRequest.MutateTrainee req = new SessionRequest.MutateTrainee
            {
                SessionId = Session.Id,
                MutateTrainees = ses

            };
            var res = await SessionService.RemoveTrainee(req);
            Session.Trainees?.Remove(user);
            hasJoined = false;
            isFull = Session.Trainees?.Count >= 6;

            if (res != null) {
                string message = "U bent succesvol verwijderd van de training!";
                var param = new DialogParameters();
                param.Add("Severity", Severity.Success);
                param.Add("ContentText", message);

                DialogService.Close(param);
            }
            else {
                string message = "Er is een fout opgetreden bij het verlaten van een training!";
                var param = new DialogParameters();
                param.Add("Severity", Severity.Success);
                param.Add("ContentText", message);

                DialogService.Close(param);
            }

        }

    }
}