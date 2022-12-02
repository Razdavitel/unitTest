using FluentValidation;

namespace Squads.Shared.Exercises;
public static class ExerciseDto
{
    public class Index
    {
        public int Id { get; set; }
        public int ExerciseType { get; set; }
    }
    public class Detail : Index
    {
        public int Repetitions { get; set; }
        public decimal Weight { get; set; }
    }
    public class Mutate
    {
        public int Repetitions { get; set; }
        public decimal Weight { get; set; }

        public class Validator : AbstractValidator<Mutate>
        {

            public Validator()
            {

            }
        }
    }
}
