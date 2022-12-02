using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Subscriptions;
using Domain.TurnCards;

namespace Domain.Payments;

public class Payment : Entity
{
    private decimal _price;
    public decimal Price 
    { 
        get => _price;
        set => _price = Guard.Against.Negative(value);
    }
    public DateTime? PayedAt { get; set; }
    public int? SubscriptionId { get; set; }
    private Subscription? _subscription;
    public Subscription? Subscription 
    { 
        get => _subscription; 
        set 
        {
            if(TurnCard != null)
                throw new InvalidDataException(
                    String.Format("Cannot link subscription, Payment {0} already linked to a subscription", Id)
                );

            _subscription = value;
        } 
    }

    public int? TurnCardId { get; set; }
    private TurnCard? _turnCard;
    public TurnCard? TurnCard 
    { 
        get => _turnCard; 
        set 
        {
            if(Subscription != null)
                throw new InvalidDataException(
                    String.Format("Cannot link turncard, Payment {0} already linked to a subscription", Id)
                );

            _turnCard = value;
        }
    }
    public DateTime CreatedOn { get; set;  }
    public DateTime? UpdatedOn { get; set;  }

    private Payment(){}

    public Payment(decimal price, Subscription subscription)
    {
        this.Price = price;
        
        this.Subscription = subscription;
        this.SubscriptionId = subscription.Id;
    }

    public Payment(decimal price, TurnCard turnCard)
    {
        this.Price = price;
        this.TurnCard = turnCard;
        this.TurnCardId = turnCard.Id;
    }

    public bool isPayed()
    {
        return PayedAt != null;
    }
}