using Squads.Shared.TurnCards;
using Domain.TurnCards;

namespace Squads.Server.Mapper;

public class TurnCardMapperProfile : UserMapperProfile
{
    public TurnCardMapperProfile()
    {
        CreateMap<TurnCard, TurnCardDto.Detail>();
        CreateMap<TurnCard, TurnCardDto.Index>();
        CreateMap<TurnCardDto.Mutate, TurnCard>(); 
    }
}