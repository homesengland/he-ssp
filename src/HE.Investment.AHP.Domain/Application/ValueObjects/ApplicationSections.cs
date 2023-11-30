using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationSections : ValueObject
{
    public ApplicationSections(IList<ApplicationSection> sections)
    {
        Sections = sections;
    }

    public IList<ApplicationSection> Sections { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Sections;
    }
}
