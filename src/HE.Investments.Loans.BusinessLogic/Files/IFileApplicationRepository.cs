using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Files;

public interface IFileApplicationRepository
{
    Task<string> GetBaseFilePath(LoanApplicationId applicationId, CancellationToken cancellationToken);
}
