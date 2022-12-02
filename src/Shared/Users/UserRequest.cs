using Squads.Shared.Common;

namespace Squads.Shared.Users;

public static class UserRequest
{
    public class GetIndex : CommonSearchRequest{}

    public class GetDetail 
    {
       public int UserId { get; set; }
       public string Email { get; set; } = string.Empty;
    }

    public class Delete
    {
        public int UserId { get; set; }
    }

    public class Create
    {
        public UserDto.Mutate user { get; set; }
    }

    public class Edit
    {
        public int UserId { get; set; }
        public UserDto.Mutate User { get; set; }
    }

    public class InviteUser
    {
        public UserDto.Create User { get; set; }
    }

    public class ActivateUser
    {
        public UserDto.Activate User { get; set; }
    }
}
