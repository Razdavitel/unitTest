using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Sessions;
using Domain.Workouts;
using Domain.PricePlans;
using Domain.Subscriptions;
using Domain.TurnCards;
using Domain.Payments;

namespace Domain.Users;
public class User : Entity
{
    #region FIELDS
    private List<Session> sessions = new();
    private List<Workout> workouts = new();
    #endregion

    #region PROPERTIES
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public String? Address{ get; set; }
    public String? PhoneNumber{ get; set; }
    public RoleType Role { get; set; } = RoleType.Customer;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public DateTime? CreatedOn { get; set; }
    public IReadOnlyCollection<Session> Sessions => sessions.AsReadOnly();
    public PricePlan ActivePricePlan {get; set;}
    public List<Subscription> Subscriptions { get; set; }
    public List<TurnCard> TurnCards { get; set; }
    public UserStatus Status { get; set; }
    #endregion

    #region CONSTRUCTORS
    private User() { }
    public User(
        String firstname, 
        String lastname, 
        String email, 
        DateTime dateOfBirth,
        byte[] passwordHash,
        byte[] passwordSalt,
        RoleType role,
        PricePlan activePricePlan,
        UserStatus status
    )
    {
        Email = Guard.Against.InvalidFormat(email, nameof(email), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        FirstName = Guard.Against.NullOrWhiteSpace(firstname, nameof(firstname));
        LastName = Guard.Against.NullOrWhiteSpace(lastname, nameof(lastname));
        PasswordHash = Guard.Against.Null(passwordHash, nameof(passwordHash));
        PasswordSalt = Guard.Against.Null(passwordSalt, nameof(passwordSalt));
        Role = Guard.Against.Null(role);
        DateOfBirth = dateOfBirth;
        ActivePricePlan = activePricePlan;
        Status = status;
    }
    public void AddUserWorkout(Workout workout) {
        workouts.Add(workout);
    }
    public void RemoveUserWorkout(Workout workout)
    {
        workouts.Remove(workout);
    }
    #endregion

    public decimal GetSubscriptionPrice()
    {
        return ActivePricePlan.SubscriptionPrice;
    }

    public decimal GetTurnCardPrice()
    {
        return ActivePricePlan.TurnPrice;
    }

    public Subscription ExtendSubscription()
    {
        Subscription last = GetLastSubscription();
        bool isExpired = last.ValidTill < DateTime.Now;
        DateTime newEndDate;
        if(last.ValidTill < DateTime.Now)
        {
            newEndDate = DateTime.Today.AddMonths(1);
        }
        else
        {
            newEndDate = last.ValidTill.AddMonths(1);
        }
        Subscription newSub = new(this, newEndDate);
        Subscriptions.Add(newSub);
        return newSub;
    }

    public Subscription GetLastSubscription()
    {
        Subscriptions
            .Sort((s1, s2) => s1.ValidTill.CompareTo(s2.ValidTill));

        Subscription last = Subscriptions.Last();
        return last;
    }

    public List<Payment> GetPayments()
    {
        List<Payment> payments = new();
        payments.AddRange(Subscriptions.Select(s => s.Payment));
        payments.AddRange(TurnCards.Select(t => t.Payment));
        return payments;
    }

    public TurnCard CreateTurnCard()
    {
        TurnCard turnCard = new(this, DateTime.Today.AddYears(1));
        TurnCards.Add(turnCard);
        return turnCard;
    }
}
