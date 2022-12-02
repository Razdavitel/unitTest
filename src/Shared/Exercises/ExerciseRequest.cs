

using Squads.Shared.Workouts;

namespace Squads.Shared.Exercises;
public static class ExerciseRequest
{
    public class GetIndex
    {
        public bool IsDone { get; set; }
        public int? Page { get; set; }
        public int? Amount { get; set; } = 25;
    }
    public class GetDetail
    {
        public int WorkoutId { get; set; }
    }
    public class Delete
    {
        public int WorkoutId { get; set; }
    }
    public class Create
    {
        public WorkoutDto.Mutate Workout { get; set; }
    }
    public class Edit
    {
        public int WorkoutId { get; set; }
        public WorkoutDto.Mutate Workout { get; set; }
    }
}
