using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Application.ValueObjects;

public class UserLoanApplication : ValueObject
{
    public UserLoanApplication(LoanApplicationId id, LoanApplicationName applicationName, ApplicationStatus status, DateTime? createdOn, DateTime? lastModificationDate)
    {
        Id = id;
        ApplicationName = applicationName;
        Status = status;
        CreatedOn = createdOn;
        LastModificationDate = lastModificationDate;
    }

    public LoanApplicationId Id { get; }

    public LoanApplicationName ApplicationName { get; }

    public ApplicationStatus Status { get; }

    public DateTime? LastModificationDate { get; }

    public DateTime? CreatedOn { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return ApplicationName;
        yield return Status;
        yield return LastModificationDate!;
    }
}
