using HE.Investments.Loans.Contract.CompanyStructure.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Loans.BusinessLogic.CompanyStructure;

public interface ICompanyStructureFileFactory
{
    OrganisationMoreInformationFile Create(IFormFile file);

    OrganisationMoreInformationFile Create(FileToUpload file);
}
