using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Workouts;

namespace Domain.Exercises;
public class Exercise : Entity
{
    private List<Workout> workouts = new();
    public decimal? Weight { get; set; }
    public int? Repetitions { get; set; }
    public int ExerciseType { get; set; }
    public IReadOnlyCollection<Workout> Workouts => workouts.AsReadOnly();

    private Exercise()
    {

    }
    public Exercise(int exerciseType)

    {
        var types = Enum.GetValues<ExerciseType>();

        types.GetValue(exerciseType);
        ExerciseType = exerciseType;
    }
}
