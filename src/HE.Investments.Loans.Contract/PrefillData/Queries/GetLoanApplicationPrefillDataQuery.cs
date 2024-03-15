using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.PrefillData.Queries;

public record GetLoanApplicationPrefillDataQuery(LoanApplicationId ApplicationId) : IRequest<LoanApplicationPrefillData>;
