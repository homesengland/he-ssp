using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Consortium.Contract;

public class ProgrammeSlim : ValueObject
{
    public ProgrammeSlim(ProgrammeId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Programme Name cannot be empty");
        }

        Id = id;
        Name = name;
    }

    public ProgrammeId Id { get; }

    public string Name { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
    }
}
