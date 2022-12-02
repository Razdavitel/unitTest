namespace Squads.Shared.PricePlans;

public static class PricePlanResponse
{
    public class GetIndex
    {
        public List<PricePlanDto.Detail> PricePlans { get; set; } = new();
        public int TotalAmount { get; set; }
    }

    public class GetDetail
    {
        public PricePlanDto.Detail PricePlan { get; set; }
    }

    public class Delete {}

    public class Create : GetDetail {}
    public class Edit : GetDetail {}

}
