using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;

public record CompleteFinancialDetailsCommand(
    ApplicationId ApplicationId) : IRequest<OperationResult>;
