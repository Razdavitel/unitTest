namespace Squads.Shared.PricePlans;

public static class PricePlanDto
{
    public class Detail 
    {
        public int Id {get; set;}
        public string name {get; set;} = string.Empty;
        public decimal SubscriptionPrice {get; set;}
        public decimal TurnPrice {get; set;}
        public DateTime? endDate {get; set;}
    }

    public class Mutate
    {
        public string name {get; set;} = string.Empty;
        public decimal SubscriptionPrice {get; set;}
        public decimal TurnPrice {get; set;}
        public DateTime? endDate {get; set;}
    }
}
