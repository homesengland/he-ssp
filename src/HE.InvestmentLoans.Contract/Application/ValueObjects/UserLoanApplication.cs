using Dawn;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Application.ValueObjects;

public class UserLoanApplication : ValueObject
{
    public UserLoanApplication(LoanApplicationId id, string applicationName, string status, DateTime? lastModificationDate)
    {
        Id = id;
        ApplicationName = applicationName;
        Status = Guard.Argument(status, nameof(Status)).NotEmpty();
        LastModificationDate = lastModificationDate;
    }

    public LoanApplicationId Id { get; }

    public string ApplicationName { get; }

    public string Status { get; }

    public DateTime? LastModificationDate { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return ApplicationName;
        yield return Status;

        if (LastModificationDate.HasValue)
        {
            yield return LastModificationDate.Value;
        }
    }
}
