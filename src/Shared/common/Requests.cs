namespace Squads.Shared.Common;

public class CommonSearchRequest
{
    public string? SearchTerm { get; set; }
    public int? Page { get; set; }
    public int? Amount { get; set; }
}

public class IdRequest 
{
    public int Id { get; set; }
}