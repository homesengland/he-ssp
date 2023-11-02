using HE.InvestmentLoans.Contract.Application.Enums;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.Contract.Application.ValueObjects;

public class UserLoanApplication : ValueObject
{
    public UserLoanApplication(LoanApplicationId id, LoanApplicationName applicationName, ApplicationStatus status, DateTime? createdOn, DateTime? lastModificationDate, string lastModifiedBy)
    {
        Id = id;
        ApplicationName = applicationName;
        Status = status;
        CreatedOn = createdOn;
        LastModificationDate = lastModificationDate;
        LastModifiedBy = lastModifiedBy;
    }

    public LoanApplicationId Id { get; }

    public LoanApplicationName ApplicationName { get; }

    public ApplicationStatus Status { get; }

    public DateTime? LastModificationDate { get; }

    public string LastModifiedBy { get; }

    public DateTime? CreatedOn { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return ApplicationName;
        yield return Status;
        yield return LastModificationDate!;
        yield return LastModifiedBy;
    }
}
