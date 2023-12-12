using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;

public record CompleteFinancialDetailsCommand(ApplicationId ApplicationId, IsSectionCompleted IsSectionCompleted) : IRequest<OperationResult>;
