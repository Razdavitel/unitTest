using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common;
public class Address : ValueObject
{
    public string Country { get; }
    public string City { get; }
    public string Postalcode { get; }
    public string Street { get; }

    public Address(string country, string city, string postalcode, string street)
    {
        Country = Guard.Against.NullOrEmpty(country, nameof(country));
        City = Guard.Against.NullOrEmpty(city, nameof(city));
        Postalcode = Guard.Against.NullOrEmpty(postalcode, nameof(postalcode));
        Street = Guard.Against.NullOrEmpty(street, nameof(street));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street.ToLower();
        yield return Country.ToLower();
        yield return Postalcode.ToLower();
        yield return City.ToLower();
    }
}
