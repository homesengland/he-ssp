using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;
public record StartFinancialDetailsCommand(Guid ApplicationId, string ApplicationName) : IRequest<OperationResult>;
