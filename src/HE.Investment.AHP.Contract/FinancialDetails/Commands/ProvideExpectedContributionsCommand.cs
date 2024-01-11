using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;

public record ProvideExpectedContributionsCommand(
    AhpApplicationId ApplicationId,
    string? RentalIncomeBorrowing,
    string? SalesOfHomesOnThisScheme,
    string? SalesOfHomesOnOtherSchemes,
    string? OwnResources,
    string? RcgfContribution,
    string? OtherCapitalSources,
    string? SharedOwnershipSales,
    string? HomesTransferValue) : IRequest<OperationResult>;
