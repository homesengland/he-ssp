using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Organization.Commands;

public record CreateAndLinkOrganisationCommand(
    string? Name,
    string? AddressLine1,
    string? AddressLine2,
    string? TownOrCity,
    string? County,
    string? Postcode) : IRequest<OperationResult>;
