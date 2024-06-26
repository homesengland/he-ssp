namespace HE.Investments.Common.Contract;

public record ModificationDetails(string? ChangedByFirstName, string? ChangedByLastName, DateTime? ChangedOn)
{
    public string ChangedBy => $"{ChangedByFirstName} {ChangedByLastName}";
}
