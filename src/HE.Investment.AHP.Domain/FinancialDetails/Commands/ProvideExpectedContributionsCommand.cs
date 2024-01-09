using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;

public record ProvideExpectedContributionsCommand(
    ApplicationId ApplicationId,
    string? RentalIncomeBorrowing,
    string? SalesOfHomesOnThisScheme,
    string? SalesOfHomesOnOtherSchemes,
    string? OwnResources,
    string? RCGFContribution,
    string? OtherCapitalSources,
    string? SharedOwnershipSales,
    string? HomesTransferValue) : IRequest<OperationResult>;
