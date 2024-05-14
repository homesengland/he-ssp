using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideOwnerOfTheLandCommand(SiteId SiteId, OrganisationId OrganisationId, bool? IsConfirmed)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
