using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Queries;

public record GetUserOrganisationInformationQuery() : IRequest<GetUserOrganisationInformationQueryResponse>;

public record GetUserOrganisationInformationQueryResponse(
    OrganizationBasicInformation OrganizationBasicInformation,
    string UserFirstName,
    bool IsLimitedUser,
    IList<UserLoanApplication> LoanApplications);
