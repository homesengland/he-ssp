using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

public class OrganisationMoreInformationFile : ValueObject
{
    public byte[] Content { get; private set; }

    public string FileName { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return FileName;
        yield return Content;
    }
}
