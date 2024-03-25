using HE.Investments.Loans.BusinessLogic.Config;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Documents;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Loans.BusinessLogic.LoanApplication;

public class SupportingDocumentsFileFactory : ISupportingDocumentsFileFactory
{
    private readonly ILoansDocumentSettings _documentSettings;

    public SupportingDocumentsFileFactory(ILoansDocumentSettings documentSettings)
    {
        _documentSettings = documentSettings;
    }

    public SupportingDocumentsFile Create(IFormFile file)
    {
        return new SupportingDocumentsFile(file.FileName, file.Length, _documentSettings.MaxFileSizeInMegabytes, file.OpenReadStream());
    }

    public SupportingDocumentsFile Create(FileToUpload file)
    {
        return new SupportingDocumentsFile(file.Name, file.Length, _documentSettings.MaxFileSizeInMegabytes, file.Content);
    }
}
