using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Commands;

public record CreateAndLinkOrganisationCommand(
    string? Name,
    string? AddressLine1,
    string? AddressLine2,
    string? TownOrCity,
    string? County,
    string? Postcode) : IRequest<OperationResult>;
