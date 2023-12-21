using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Commands;

public record CreateApplicationCommand(string? Name, Tenure Tenure) : IRequest<OperationResult<ApplicationId>>;
