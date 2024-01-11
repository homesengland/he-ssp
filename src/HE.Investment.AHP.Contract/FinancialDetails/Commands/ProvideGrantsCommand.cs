using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;

public record ProvideGrantsCommand(
    AhpApplicationId ApplicationId,
    string? CountyCouncilGrants,
    string? DhscExtraCareGrants,
    string? LocalAuthorityGrants,
    string? SocialServicesGrants,
    string? HealthRelatedGrants,
    string? LotteryGrants,
    string? OtherPublicBodiesGrants) : IRequest<OperationResult>;
