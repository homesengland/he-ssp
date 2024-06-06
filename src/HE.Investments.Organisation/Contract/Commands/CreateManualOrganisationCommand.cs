using HE.Investments.Common.Contract.Validators;
using HE.Investments.Organisation.ValueObjects;
using MediatR;

namespace HE.Investments.Organisation.Contract.Commands;

public record CreateManualOrganisationCommand(
    string? Name,
    string? AddressLine1,
    string? AddressLine2,
    string? TownOrCity,
    string? County,
    string? Postcode) : IRequest<OperationResult<InvestmentsOrganisation>>;
