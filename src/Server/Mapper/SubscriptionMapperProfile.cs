using Squads.Shared.Subscriptions;
using Domain.Subscriptions;

namespace Squads.Server.Mapper;

public class SubscriptionMapperProfile : UserMapperProfile
{
    public SubscriptionMapperProfile()
    {
        CreateMap<Subscription, SubscriptionDto.Detail>();
        CreateMap<Subscription, SubscriptionDto.Index>();
        CreateMap<SubscriptionDto.Mutate, Subscription>(); 
    }
}