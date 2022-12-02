using Radzen;
using Microsoft.AspNetCore.Components;
using Squads.Shared.Sessions;
using Squads.Shared.Workouts;
using Squads.Shared.Users;

namespace Squads.Client.Planner.Components
{
    public partial class EditAppointmentPage
    {
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public ISessionService SessionService { get; set; }
        [Inject] public IWorkoutService WorkoutService { get; set; }
        [Inject] public IUserService UserService { get; set; }

        [Parameter]
        public DateTime Start { get; set; }

        [Parameter]
        public DateTime End { get; set; }
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Description { get; set; }
        [Parameter]
        public SessionDto.Detail Session { get; set; }

        private SessionDto.Mutate model = new();

        private IList<UserDto.Index> coaches = new List<UserDto.Index>();
        private IList<WorkoutDto.Index> workouts = new List<WorkoutDto.Index>();
        protected override async Task OnInitializedAsync()
        {
            UserRequest.GetIndex request = new();
            var response = await UserService.GetIndexAsync(request);
            coaches = response.Users.Where(c => c.Role == 2).ToList();

            WorkoutRequest.GetIndex req = new();
            var res = await WorkoutService.GetIndexAsync(req);
            workouts = res.Workouts;

        }
        protected void OnChangeCoach(object value)
        {
            var id = value;
            model.CoachId = (int) id;

        }
        protected void OnChangeWorkout(object value)
        {
            var id = value;
            model.WorkoutId = (int)id;
        }
        protected override void OnParametersSet()
        {
            model.StartsAt = Session.StartsAt;
            model.EndsAt = Session.EndsAt;
            model.Description=Session.Description;
            model.Title=Session.Title;
            model.CoachId = Session.Coach?.Id;
            model.WorkoutId = Session.Workout?.Id;
        }

        void OnSubmit(SessionDto.Mutate model)
        {
            DialogService.Close(model);
        }
        private async Task DeleteSessionAsync()
        {
            var request = new SessionRequest.Delete { SessionId = Session.Id };
            await SessionService.DeleteAsync(request);
            DialogService.Close(model);
        }
    }
}