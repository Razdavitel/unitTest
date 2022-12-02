using Domain.Common;
using Domain.Exercises;

namespace Domain.Workouts;
public class Workout : Entity
{
    private List<Exercise> exercises = new();
    public int WorkoutType { get; set; }
    public DateTime Date { get; set; }

    public IReadOnlyCollection<Exercise> Exercises => exercises.AsReadOnly();


    private Workout()
    {

    }
    public Workout(int workoutType, DateTime date)

    {
        var types = Enum.GetValues<WorkoutType>();
        types.GetValue(workoutType);
        WorkoutType = workoutType;
        Date = date;
    }

    public void AddExercise(Exercise exercise) { 
        exercises.Add(exercise);  
    }
    public void RemoveExercise(Exercise exercise)
    {
        exercises.Remove(exercise);
    }

}
