using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Projects.Commands;

public record ProvideAdditionalDetailsCommand(
    LoanApplicationId LoanApplicationId,
    ProjectId ProjectId,
    DateDetails? PurchaseDate,
    string Cost,
    string CurrentValue,
    string SourceOfValuation) : IRequest<OperationResult>;
