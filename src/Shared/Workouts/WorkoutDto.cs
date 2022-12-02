using FluentValidation;
using Squads.Shared.Record;
using Squads.Shared.Sessions;

namespace Squads.Shared.Workouts;
public static class WorkoutDto
{
    public class Index
    {
        public int Id { get; set; }
        public int WorkoutType { get; set; }
    }
    public class Detail : Index
    {
        public DateTime Date { get; set; }

        //public List<ExerciseDto.Index>? Exercises { get; set; }
    }
    public class Mutate
    {
        public DateTime Date { get; set; }
        public int WorkoutType { get; set; }

        public class Validator : AbstractValidator<Mutate>
        {
            public Validator()
            {
               // RuleFor(x => x.).NotEmpty().Length(1, 250);
            }
        }
    }
}
