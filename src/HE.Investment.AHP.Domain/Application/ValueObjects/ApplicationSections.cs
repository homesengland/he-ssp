using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationSections : ValueObject
{
    public ApplicationSections(
        SectionStatus schemeStatus = SectionStatus.NotStarted,
        SectionStatus homeTypesStatus = SectionStatus.NotStarted,
        SectionStatus financialDetailsStatus = SectionStatus.NotStarted,
        SectionStatus deliveryPhasesStatus = SectionStatus.NotStarted)
    {
        SchemeStatus = schemeStatus;
        HomeTypesStatus = homeTypesStatus;
        FinancialDetailsStatus = financialDetailsStatus;
        DeliveryPhasesStatus = deliveryPhasesStatus;
    }

    public SectionStatus SchemeStatus { get; }

    public SectionStatus HomeTypesStatus { get; }

    public SectionStatus FinancialDetailsStatus { get; }

    public SectionStatus DeliveryPhasesStatus { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return SchemeStatus;
        yield return HomeTypesStatus;
        yield return FinancialDetailsStatus;
        yield return DeliveryPhasesStatus;
    }
}
