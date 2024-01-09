using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Security.Commands;

public record ProvideCompanyDebenture(LoanApplicationId Id, string Exists, string Holder) : IRequest<OperationResult>;
