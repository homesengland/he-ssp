using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record CompleteSiteCommand(SiteId SiteId, IsSectionCompleted IsSectionCompleted) : IRequest<OperationResult>, IProvideSiteDetailsCommand;
