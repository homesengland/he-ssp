using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.CompanyStructure.ValueObjects;

public class OrganisationMoreInformationFile : ProvidableValueObject<OrganisationMoreInformationFile>
{
    public OrganisationMoreInformationFile(string fileName, byte[] content)
    {
        FileName = fileName;
        Content = content;
    }

    public string FileName { get; }

    public byte[] Content { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return FileName;
        yield return Content;
    }
}
