using Squads.Shared.Payments;
using Squads.Shared.Users;

namespace Squads.Shared.TurnCards;

public static class TurnCardDto
{
    public class Index
    {
        public int Id { get; set; }
        public int NumberOfTurns { get; set; }
        public DateTime ValidTill { get; set; }
    }

    public class Detail : Index
    {
        public PaymentDto.Index Payment { get; set; }
        public UserDto.Index Customer { get; set; }
    }

    public class Mutate
    {
        public int? NumberOfTurns { get; set; }
        public int? CustomerId { get; set; }
    }

}
