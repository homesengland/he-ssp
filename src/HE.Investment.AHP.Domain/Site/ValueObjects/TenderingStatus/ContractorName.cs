using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;

public class ContractorName : ShortText
{
    public ContractorName(string? value)
        : base(value, "ContractorName", "name of the contractor")
    {
    }

    public static ContractorName? Create(string? value) => !string.IsNullOrWhiteSpace(value) ? new ContractorName(value) : null;
}
