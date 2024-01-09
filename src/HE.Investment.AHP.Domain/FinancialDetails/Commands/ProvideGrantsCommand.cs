using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;

public record ProvideGrantsCommand(
    ApplicationId ApplicationId,
    string? CountyCouncilGrants,
    string? DHSCExtraCareGrants,
    string? LocalAuthorityGrants,
    string? SocialServicesGrants,
    string? HealthRelatedGrants,
    string? LotteryGrants,
    string? OtherPublicBodiesGrants) : IRequest<OperationResult>;
