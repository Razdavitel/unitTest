using FluentValidation;
using Squads.Shared.Users;
using Squads.Shared.Workouts;

namespace Squads.Shared.Sessions;

public static class SessionDto
{
    public class Index
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public WorkoutDto.Index Workout { get; set; }
        public List<UserDto.Index>? Trainees { get; set; }
    }

    public class Detail : Index
    {
        public string Description { get; set; }
        public UserDto.Index Coach { get; set; }
    }
    public class Mutate
    {
        public int? CoachId { get; set; }
        public int? WorkoutId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public int? TraineeId { get; set; }
        //public List<UserDto.Index>? Trainees { get; set; }
        public class Validator : AbstractValidator<Mutate>
        {

            public Validator()
            {
                RuleFor(x => x.Title).NotEmpty().Length(1, 250);
                RuleFor(x => x.EndsAt).GreaterThan(DateTime.Now);
                RuleFor(x => x.StartsAt).GreaterThan(DateTime.Now);
            }
        }
    }
        public class MutateTrainees
        {
            public int TraineeId { get; set; }
            public class Validator : AbstractValidator<Mutate>
            {
                public Validator()
                {

                }
            }

        }
  }
