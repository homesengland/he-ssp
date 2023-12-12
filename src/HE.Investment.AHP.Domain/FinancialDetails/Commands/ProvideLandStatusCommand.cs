using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;

public record ProvideLandStatusCommand(ApplicationId ApplicationId, string? PurchasePrice, bool IsFinal) : IRequest<OperationResult>;
