using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record StartSiteCommand(SiteId SiteId) : IRequest<OperationResult>, IProvideSiteDetailsCommand;
