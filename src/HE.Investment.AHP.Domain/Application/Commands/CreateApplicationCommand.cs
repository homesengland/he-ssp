using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.Application.Commands;

public record CreateApplicationCommand(string Name) : IRequest<OperationResult<ApplicationId?>>;
