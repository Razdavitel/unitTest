using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Users;
using Domain.Workouts;

namespace Domain.Sessions;

public class Session : Entity
{
    #region FIELDS
    private string title;
    private string description;
    private DateTime startsAt;
    private DateTime endsAt;
    private List<User> trainees = new();
    #endregion

    #region PROPERTIES
    public User Coach { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Workout Workout { get; set; }

    public DateTime StartsAt
    {
        get { return startsAt; }
        set { startsAt = Guard.Against.Null(value, nameof(startsAt)); }
    }
    public DateTime EndsAt
    {
        get { return endsAt; }
        set { endsAt = Guard.Against.Null(value, nameof(endsAt)); }
    }

    public string Title
    {
        get { return title; }
        set { title = Guard.Against.NullOrWhiteSpace(value, nameof(title)); }
    }

    public string Description
    {
        get { return description; }
        set { description = Guard.Against.NullOrWhiteSpace(value, nameof(description)); }
    }

    public IReadOnlyCollection<User> Trainees => trainees.AsReadOnly();
    #endregion

    #region CONSTRUCTORS
    private Session() { }

    public Session(string title, string description, DateTime startsAt, DateTime endsAt, User coach, Workout workout)
    {
        Title = title;
        Description = description;
        StartsAt = startsAt;
        EndsAt = endsAt;
        Coach = Guard.Against.Null(coach);
        Workout = Guard.Against.Null(workout);
    }
    #endregion

    #region METHODS
    public void AddReservation(User user)
    {
        if (trainees.Count >= 6) throw new ArgumentException("Session is fully booked");
        trainees.Add(user);
        user.AddUserWorkout(Workout);
    }
    public void RemoveReservation(User user)
    {
        trainees.Remove(user);
        user.RemoveUserWorkout(Workout);
    }
    #endregion
}



