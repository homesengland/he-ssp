using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;
public record StartFinancialDetailsCommand(ApplicationId ApplicationId, string ApplicationName) : IRequest<OperationResult>;
