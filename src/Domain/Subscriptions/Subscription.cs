using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Users;
using Domain.Payments;

namespace Domain.Subscriptions;
public class Subscription : Entity
{
    public virtual User Customer { get; set; }
    public DateTime ValidTill { get; set; }
    public Payment Payment {get; set;}
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    private Subscription(){}
    public Subscription(User customer, DateTime validTill)
    {
        Customer = Guard.Against.Null(customer, nameof(customer));
        ValidTill = validTill;
    }

    public Payment CreatePayment()
    {   
        if(Payment != null)
            return Payment;
            
        Payment payment = new(Customer.GetSubscriptionPrice(), this);
        this.Payment = payment;
        return payment;
    }
}
