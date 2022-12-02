namespace Squads.Shared.Subscriptions;

public interface ISubscriptionService
{
    SubscriptionResponse.GetIndex GetList(SubscriptionRequest.GetIndex request);
    SubscriptionResponse.GetDetail Get(SubscriptionRequest.GetDetail request);
    SubscriptionResponse.GetDetail Create(SubscriptionRequest.Create request);
    SubscriptionResponse.GetDetail Edit(SubscriptionRequest.Edit request);
    void Delete(SubscriptionRequest.Delete request);
    SubscriptionResponse.GetDetail? GetLastSubscription(int userId);
}
