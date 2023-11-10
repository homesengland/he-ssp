using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;

public record LinkContactWithOrganizationCommand(CompaniesHouseNumber Number) : IRequest;
