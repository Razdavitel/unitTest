using Squads.Shared.Common;

namespace Squads.Shared.PricePlans;

public static class PricePlanRequest
{
    public class GetIndex : CommonSearchRequest {}
    public class GetDetail : IdRequest {}

    public class Create 
    {
        public PricePlanDto.Mutate PricePlan { get; set; }
    }

    public class Edit : IdRequest
    {
        public PricePlanDto.Mutate ChangedPricePlan {get; set;}
    }

    public class Delete : IdRequest {}
}
