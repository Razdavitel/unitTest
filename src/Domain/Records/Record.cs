using Domain.Common;
using Domain.Users;

namespace Domain.Records;
public class Record : Entity
{
    #region FIELDS
    private List<string> physicalIssues = new ();
    private List<string> medications = new ();
    #endregion

    #region PROPERTIES
    // is not readonly because of converter
    public IEnumerable<string> PhysicalIssues => physicalIssues;
    public IEnumerable<string> Medications => medications;
    public DateTime? UpdatedOn { get; set; }
    public DateTime? CreatedOn { get; set; }

    public int Height { get; set; }
    public User Customer { get; set; }
    #endregion

    #region CONSTRUCTORS
    public Record()
    {

    }
    public Record(int height, User user)
    {
        Height = height;
        Customer = user;
    }
    public Record(int height, User user, List<string> physicalIssues, List<string> medications)
    {
        Height = height;
        Customer = user;
        this.physicalIssues.AddRange(physicalIssues);
        this.medications.AddRange(medications);
    }

    #endregion

    #region METHODS
    public void AddPhysicalIssues(string issue)
    {
        physicalIssues.Add(issue);
    }
    public void AddMedications(string medication)
    {
        medications.Add(medication);
    }
    public void RemovePhysicalIssues(string issue)
    {
        physicalIssues.Remove(issue);
    }
    public void RemoveMedications(string medication)
    {
        medications.Remove(medication);
    }
    #endregion
}
