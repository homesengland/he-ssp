using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure;

public class CompanyStructureFileFactory : ICompanyStructureFileFactory
{
    private readonly ILoansDocumentSettings _documentSettings;

    public CompanyStructureFileFactory(ILoansDocumentSettings documentSettings)
    {
        _documentSettings = documentSettings;
    }

    public OrganisationMoreInformationFile Create(IFormFile file)
    {
        return new OrganisationMoreInformationFile(file.FileName, file.Length, _documentSettings.MaxFileSizeInMegabytes, file.OpenReadStream());
    }

    public OrganisationMoreInformationFile Create(FileToUpload file)
    {
        return new OrganisationMoreInformationFile(file.Name, file.Lenght, _documentSettings.MaxFileSizeInMegabytes, file.Content);
    }
}
