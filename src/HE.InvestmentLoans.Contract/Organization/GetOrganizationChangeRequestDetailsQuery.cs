using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Organization;

public record GetOrganizationChangeRequestDetailsQuery() : IRequest<GetOrganizationChangeRequestDetailsQueryResponse>;

public record GetOrganizationChangeRequestDetailsQueryResponse(string ChangeRequestDetails);
