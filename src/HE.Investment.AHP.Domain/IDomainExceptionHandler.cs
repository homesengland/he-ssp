using HE.InvestmentLoans.Common.Validation;

namespace HE.Investment.AHP.Domain;

public interface IDomainExceptionHandler
{
    Task<OperationResult<TResult>> Handle<TResult>(Func<Task<OperationResult<TResult>>> action);
}
