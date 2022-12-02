namespace Squads.Shared.Exercises;
public static class ExerciseResponse
{
    public class GetIndex
    {
        public int TotalAmount { get; set; }
        public List<ExerciseDto.Index> Exercises { get; set; } = new();
    }
    public class GetDetail
    {
        public ExerciseDto.Detail Exercise { get; set; }
    }
    public class Delete
    {
    }
    public class Create
    {
        public int ExerciseId { get; set; }
    }
    public class Edit
    {
        public int ExerciseId { get; set; }
    }

}
