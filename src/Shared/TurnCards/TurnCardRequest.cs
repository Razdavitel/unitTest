using Squads.Shared.Common;

namespace Squads.Shared.TurnCards;

public static class TurnCardRequest
{
    public class GetIndex : CommonSearchRequest
    {
        public int? UserId { get; set; }
    }

    public class GetDetail : IdRequest{}

    public class Create 
    {
        public TurnCardDto.Mutate TurnCard { get; set; }
    }

    public class Delete : IdRequest {}
    
    public class Edit : IdRequest
    {
        public TurnCardDto.Mutate TurnCard { get; set; }
    }
}
