using Squads.Shared.Payments;
using Domain.Payments;

namespace Squads.Server.Mapper;

public class PaymentMapperProfile : UserMapperProfile
{
    public PaymentMapperProfile()
    {
        CreateMap<Payment, PaymentDto.Detail>();
        CreateMap<Payment, PaymentDto.Index>();
        CreateMap<PaymentDto.Mutate, Payment>(); 
    }
}