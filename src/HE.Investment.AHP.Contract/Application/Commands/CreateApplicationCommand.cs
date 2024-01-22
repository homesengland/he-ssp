using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Application.Commands;

public record CreateApplicationCommand(SiteId siteId, string? Name, Tenure Tenure) : IRequest<OperationResult<AhpApplicationId>>;
