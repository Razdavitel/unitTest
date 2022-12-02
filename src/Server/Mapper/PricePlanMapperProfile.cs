using Squads.Shared.PricePlans;
using Domain.PricePlans;

namespace Squads.Server.Mapper;

public class PricePlanMapperProfile : UserMapperProfile 
{
    public PricePlanMapperProfile()
    {
        CreateMap<PricePlan, PricePlanDto.Detail>();
        CreateMap<PricePlan, PricePlanDto.Detail>();
        CreateMap<PricePlanDto.Mutate, PricePlan>();
    }
}