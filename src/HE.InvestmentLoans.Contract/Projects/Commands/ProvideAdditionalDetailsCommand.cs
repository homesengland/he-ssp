using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ProvideAdditionalDetailsCommand(
    LoanApplicationId LoanApplicationId,
    ProjectId ProjectId,
    string PurchaseYear,
    string PurchaseMonth,
    string PurchaseDay,
    string Cost,
    string CurrentValue,
    string SourceOfValuation) : IRequest<OperationResult>;
