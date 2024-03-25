using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Loans.Contract.Application.Helper;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Application;

public class SupportingDocumentsViewModel
{
    public LoanApplicationId LoanApplicationId { get; set; }

    public ApplicationStatus LoanApplicationStatus { get; set; }

    public byte[]? SupportingDocumentsFile { get; set; }

    public string? FileName { get; set; }

    public IList<FileModel>? SupportingDocumentsFiles { get; set; }

    public string AllowedExtensions { get; set; }

    public bool IsReadOnly()
    {
        var readonlyStatuses = ApplicationStatusDivision.GetStatusesForReadonlySupportingDocuments();
        return readonlyStatuses.Contains(LoanApplicationStatus);
    }
}
