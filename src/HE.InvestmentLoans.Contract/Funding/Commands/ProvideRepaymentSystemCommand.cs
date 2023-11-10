using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Funding.Commands;

public record ProvideRepaymentSystemCommand(LoanApplicationId LoanApplicationId, string? RefinanceOrRepay, string? AdditionalInformation)
    : IRequest<OperationResult>;
