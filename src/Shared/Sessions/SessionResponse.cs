namespace Squads.Shared.Sessions;

public static class SessionResponse
{
    public class GetIndex
    {
        public List<SessionDto.Index> Sessions { get; set; } = new();
        public int TotalAmount { get; set; }
    }

    public class GetDetail
    {
        public SessionDto.Detail Session { get; set; }
    }

    public class Delete
    {
    }

    public class Create
    {
        public int SessionId { get; set; }
    }

    public class Edit
    {
        public int SessionId { get; set; }
    }
    public class MutateTrainee
    {
        public int SessionId { get; set; }
    }
}
