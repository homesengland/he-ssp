using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;
public record ProvideContributionsCommand(
    ApplicationId ApplicationId,
    string? RentalIncomeBorrowing,
    string? SalesOfHomesOnThisScheme,
    string? SalesOfHomesOnOtherSchemes,
    string? OwnResources,
    string? RCGFContribution,
    string? OtherCapitalSources,
    string? SharedOwnershipSales,
    string? HomesTransferValue) : IRequest<OperationResult>;
