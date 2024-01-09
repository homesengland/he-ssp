using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Commands;

public record SubmitApplicationCommand(string Id) : IRequest<OperationResult<ApplicationId>>, IUpdateApplicationCommand;
