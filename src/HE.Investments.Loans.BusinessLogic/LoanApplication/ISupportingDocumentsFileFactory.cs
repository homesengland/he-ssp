using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication;

public interface ISupportingDocumentsFileFactory
{
    SupportingDocumentsFile Create(IFormFile file);

    SupportingDocumentsFile Create(FileToUpload file);
}
