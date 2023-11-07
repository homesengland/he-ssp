using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Commands;

public record UpdateApplicationTenureCommand(string Id, string Tenure) : IRequest<OperationResult<ApplicationId?>>, IUpdateApplicationCommand;
