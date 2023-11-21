using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record RemoveHomeTypeCommand(string ApplicationId, string HomeTypeId) : IRequest<OperationResult>;
