using Dawn;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Application.ValueObjects;

public class UserLoanApplication : ValueObject
{
    public UserLoanApplication(LoanApplicationId id, string applicationName, string status)
    {
        Id = id;
        ApplicationName = applicationName;
        Status = Guard.Argument(nameof(Status), status).NotEmpty();
    }

    public LoanApplicationId Id { get; }

    public string ApplicationName { get; }

    public string Status { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return ApplicationName;
        yield return Status;
    }
}
