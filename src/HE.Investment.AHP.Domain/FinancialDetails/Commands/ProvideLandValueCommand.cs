using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;

public record ProvideLandValueCommand(ApplicationId ApplicationId, string? LandOwnership, string? LandValue) : IRequest<OperationResult>;
