namespace Squads.Shared.Sessions;

public static class SessionRequest
{
    public class GetIndex
    {
        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public int? Page { get; set; }
        public int? Amount { get; set; } = 25;
    }
        public class GetDetail
    {
        public int SessionId { get; set; }
    }

    public class Delete
    {
        public int SessionId { get; set; }
    }

    public class Create
    {
        public SessionDto.Mutate Session { get; set; }
    }

    public class Edit
    {
        public int SessionId { get; set; }
        public SessionDto.Mutate Session { get; set; }
    }
    public class MutateTrainee
    {
        public int SessionId { get; set; }
        public SessionDto.MutateTrainees MutateTrainees { get; set; }
    }
}
