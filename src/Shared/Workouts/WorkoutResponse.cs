using Squads.Shared.Users;

namespace Squads.Shared.Workouts;
public static class WorkoutResponse
{
    public class GetIndex
    {
        public List<WorkoutDto.Index> Workouts { get; set; } = new();
        public int TotalAmount { get; set; }
    }
    public class GetDetail
    {
        public WorkoutDto.Detail Workout { get; set; }
    }
    public class Delete
    {
    }
    public class Create
    {
        public int WorkoutId { get; set; }
    }
    public class Edit
    {
        public int WorkoutId { get; set; }
    }
}
