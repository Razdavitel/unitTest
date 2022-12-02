namespace Squads.Shared.Workouts;
public static class WorkoutRequest
{
    public class GetIndex
    {
        public string? SearchTerm { get; set; }
        public int? Page { get; set; }
        public int? Amount { get; set; } = 50;

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
