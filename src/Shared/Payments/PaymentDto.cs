using Squads.Shared.Subscriptions;
using Squads.Shared.TurnCards;
using Squads.Shared.Users;

namespace Squads.Shared.Payments;

public static class PaymentDto
{
    public class Index
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime? PayedAt { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class Detail : Index
    {
        public SubscriptionDto.Index? Subscription { get; set; }
        public TurnCardDto.Index? TurnCard { get; set; }
    }

    public class Mutate
    {
        public decimal? Price { get; set; }
        public int? SubscriptionId { get; set; }
        public int? TurnCardId { get; set; }
    }

    public class WithUser : Index
    {
        public UserDto.Index User { get; set; }
    }
}
