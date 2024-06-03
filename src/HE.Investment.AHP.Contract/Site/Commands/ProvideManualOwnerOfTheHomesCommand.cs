using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Site.Commands;

public record ProvideManualOwnerOfTheHomesCommand(
    SiteId SiteId,
    string? Name,
    string? AddressLine1,
    string? AddressLine2,
    string? TownOrCity,
    string? County,
    string? Postcode) : IRequest<OperationResult>, IProvideSiteDetailsCommand;
