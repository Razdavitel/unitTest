using Ardalis.GuardClauses;
using Bogus.Bson;
using Domain.Common;
using Domain.Users;

namespace Domain.Reports;
public class Report : Entity
{
    public double Weight { get; private set;  }
    public double FatPercentage { get; private set; }
    public double WaistSize { get; private set; }
    public double MusclePercentage { get; private set; }
    public User Customer { get; private set; }
    public DateTime? CreatedOn { get; private set;  }

    private Report(){ }

    public Report(User customer, double weight, double fatPercentage, double musclePercentage, double waistSize)
    {
        Customer = Guard.Against.Null(customer, nameof(customer));
        Weight = Guard.Against.OutOfRange(weight, nameof(weight), 20, 300);
        FatPercentage = Guard.Against.OutOfRange(fatPercentage, nameof(fatPercentage), 0, 100);
        MusclePercentage = Guard.Against.OutOfRange(musclePercentage, nameof(musclePercentage), 0, 100);
        WaistSize = Guard.Against.OutOfRange(waistSize, nameof(waistSize), 0, 300);
    }
}
