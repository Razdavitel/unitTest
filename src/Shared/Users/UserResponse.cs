namespace Squads.Shared.Users;

public static class UserResponse
{
    public class GetIndex
    {
        public List<UserDto.Index> Users { get; set; } = new();
        public int TotalAmount { get; set; }
    }

    public class GetDetail
    {
        public UserDto.Detail User { get; set; }
    }

    public class Delete
    {
    }

    public class Create
    {
        public int UserId { get; set; }
    }

    public class Edit
    {
        public int UserId { get; set; }
    }
}
