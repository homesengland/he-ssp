using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.InvestmentLoans.Contract.UserOrganisation.Commands;

public record ChangeOrganisationDetailsCommand(
    string? Name,
    string? PhoneNumber,
    string? AddressLine1,
    string? AddressLine2,
    string? TownOrCity,
    string? County,
    string? Postcode) : IRequest<OperationResult>;
