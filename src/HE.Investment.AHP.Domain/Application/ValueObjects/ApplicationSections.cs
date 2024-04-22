using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationSections : ValueObject
{
    public ApplicationSections(IList<ApplicationSection> sections)
    {
        Sections = sections;
    }

    public SectionStatus SchemeStatus => Sections.Single(x => x.Type == SectionType.Scheme).Status;

    public SectionStatus HomeTypesStatus => Sections.Single(x => x.Type == SectionType.HomeTypes).Status;

    public SectionStatus FinancialStatus => Sections.Single(x => x.Type == SectionType.FinancialDetails).Status;

    public SectionStatus DeliveryStatus => Sections.Single(x => x.Type == SectionType.DeliveryPhases).Status;

    public IList<ApplicationSection> Sections { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Sections;
    }
}
