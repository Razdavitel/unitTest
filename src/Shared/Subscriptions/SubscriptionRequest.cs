using Squads.Shared.Common;

namespace Squads.Shared.Subscriptions;

public static class SubscriptionRequest
{
    public class GetIndex : CommonSearchRequest
    {
        public int? UserId { get; set; }
    }

    public class GetDetail : IdRequest {}

    public class Create 
    {
        public SubscriptionDto.Mutate Subscription { get; set; }
    }

    public class Delete : IdRequest {}
    
    public class Edit : IdRequest 
    {
        public SubscriptionDto.Mutate Subscription { get; set; }
    }
}
