namespace Squads.Shared.Subscriptions;

public static class SubscriptionResponse
{
    public class GetIndex
    {
        public List<SubscriptionDto.Index> Subscriptions { get; set; } = new();
        public int TotalAmount { get; set; }
    }

    public class GetDetail
    {
        public SubscriptionDto.Detail Subscription { get; set; }
    }
}
