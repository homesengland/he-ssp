using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.UserOrganisation.Commands;

public record ChangeOrganisationDetailsCommand(
    string? Name,
    string? PhoneNumber,
    string? AddressLine1,
    string? AddressLine2,
    string? TownOrCity,
    string? County,
    string? Postcode) : IRequest<OperationResult>;
