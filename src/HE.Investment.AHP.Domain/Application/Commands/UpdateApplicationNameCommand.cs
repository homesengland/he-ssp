using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Commands;

public record UpdateApplicationNameCommand(string Id, string Name) : IRequest<OperationResult<ApplicationId?>>, IUpdateApplicationCommand;
