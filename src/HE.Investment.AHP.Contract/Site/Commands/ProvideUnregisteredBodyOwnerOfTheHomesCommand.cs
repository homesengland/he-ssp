using HE.Investments.Common.Contract.Validators;
using HE.Investments.Organisation.ValueObjects;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideUnregisteredBodyOwnerOfTheHomesCommand(SiteId SiteId, OrganisationIdentifier OrganisationIdentifier, bool? IsConfirmed)
    : IRequest<OperationResult>, IProvideSiteDetailsCommand;
