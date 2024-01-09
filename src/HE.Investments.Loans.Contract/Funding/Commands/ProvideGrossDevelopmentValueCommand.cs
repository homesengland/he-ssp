using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Funding.Commands;

public record ProvideGrossDevelopmentValueCommand(LoanApplicationId LoanApplicationId, string? GrossDevelopmentValue) : IRequest<OperationResult>;
