using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.Loans.Contract.Application.ValueObjects;
public class SubmitLoanApplication : ValueObject
{
    public SubmitLoanApplication(LoanApplicationId id, string applicationName, ApplicationStatus status, bool isEnoughHomesToBuild)
    {
        Id = id;
        ApplicationName = applicationName;
        Status = status;
        IsEnoughHomesToBuild = isEnoughHomesToBuild;
    }

    public LoanApplicationId Id { get; }

    public string ApplicationName { get; }

    public ApplicationStatus Status { get; }

    public bool IsEnoughHomesToBuild { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return ApplicationName;
        yield return Status;
        yield return IsEnoughHomesToBuild;
    }
}
