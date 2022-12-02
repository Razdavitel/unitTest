
using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Users;
using Domain.Payments;

namespace Domain.TurnCards;
public class TurnCard : Entity
{
    public int NumberOfTurns { get; set; }
    public User Customer { get; private set; }
    public DateTime ValidTill{ get; private set; }
    public Payment Payment { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    private TurnCard() {}

    public TurnCard(User customer, DateTime validTill, int numberOfTurns=10)
    {
        Customer = Guard.Against.Null(customer, nameof(customer));
        NumberOfTurns = Guard.Against.Negative(numberOfTurns, nameof(numberOfTurns));
        ValidTill = validTill;
    }

    public Payment CreatePayment()
    {
        if(Payment != null)
            return Payment;

        Payment payment = new Payment(Customer.GetTurnCardPrice(), this);
        this.Payment = payment;
        return payment;
    }

    public void ConsumeTurn()
    {
        NumberOfTurns--;
    }
}
