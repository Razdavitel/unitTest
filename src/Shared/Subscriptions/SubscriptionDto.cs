using Squads.Shared.Payments;
using Squads.Shared.Users;

namespace Squads.Shared.Subscriptions;

public static class SubscriptionDto
{
    public class Index 
    {
        public int Id { get; set; }
        public DateTime ValidTill { get; set; }
    }

    public class Detail : Index
    {
        public PaymentDto.Index Payment { get; set; }
        public UserDto.Index Customer { get; set; }
    }

    public class Mutate
    {
        public decimal? Price { get; set; }
        public DateTime? ValidTill { get; set; }
        public int? customerId { get; set; }
    }
}
