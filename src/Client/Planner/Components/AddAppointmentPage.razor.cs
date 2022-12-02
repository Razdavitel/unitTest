
using Radzen;
using Microsoft.AspNetCore.Components;
using Squads.Shared.Sessions;
using Squads.Shared.Users;
using Squads.Shared.Workouts;

namespace Squads.Client.Planner.Components
{
    public partial class AddAppointmentPage
    {
        [Inject] public DialogService DialogService { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Inject] public IWorkoutService WorkoutService { get; set; }
        [Parameter]
        public DateTime Start { get; set; }

        [Parameter]
        public DateTime End { get; set; }
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Description { get; set; }

        private SessionDto.Mutate model=new();
        private IList<UserDto.Index> coaches= new List<UserDto.Index>();
        private IList<WorkoutDto.Index> workouts = new List<WorkoutDto.Index>();

        protected override async Task OnInitializedAsync()
        {
            UserRequest.GetIndex request = new();
            var response = await UserService.GetIndexAsync(request);
            coaches = response.Users.Where(c=>c.Role==2).ToList();

            WorkoutRequest.GetIndex req= new();
            var res = await WorkoutService.GetIndexAsync(req);
            workouts = res.Workouts;

        }
        protected void OnChangeCoach(object value)
        {
            var id = value;
            model.CoachId =(int) id;

        }
        protected void OnChangeWorkout(object value)
        {
            var id = value;
            model.WorkoutId = (int) id;
        }


        protected override void OnParametersSet()
        {
            model.StartsAt = Start;
            model.EndsAt = End;
        }

        void OnSubmit(SessionDto.Mutate model)
        {
            DialogService.Close(model);
        }
    }
}