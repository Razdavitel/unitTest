using Domain.Common;

namespace Domain.PricePlans;

public class PricePlan : Entity 
{
    public string Name {get; set;}
    public decimal SubscriptionPrice {get; set;}
    public decimal TurnPrice {get; set;}
    public DateTime? endDate {get; set;}

    public PricePlan(string name, decimal subscriptionPrice, decimal turnPrice)
    {
        Name = name;
        SubscriptionPrice = subscriptionPrice;
        TurnPrice = turnPrice;
    }
}