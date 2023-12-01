using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Commands;

public record UpdateApplicationNameCommand(string Id, string Name) : IRequest<OperationResult<ApplicationId>>, IUpdateApplicationCommand;
