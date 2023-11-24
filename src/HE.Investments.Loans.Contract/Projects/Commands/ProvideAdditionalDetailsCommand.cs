using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;
public record ProvideAdditionalDetailsCommand(
    LoanApplicationId LoanApplicationId,
    ProjectId ProjectId,
    string PurchaseYear,
    string PurchaseMonth,
    string PurchaseDay,
    string Cost,
    string CurrentValue,
    string SourceOfValuation) : IRequest<OperationResult>;
