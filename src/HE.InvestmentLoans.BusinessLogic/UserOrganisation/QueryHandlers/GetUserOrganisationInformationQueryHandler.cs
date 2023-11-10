using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.UserOrganisation.QueryHandlers;

public class GetUserOrganisationInformationQueryHandler : IRequestHandler<GetUserOrganisationInformationQuery, GetUserOrganisationInformationQueryResponse>
{
    private readonly IAccountUserContext _loanUserContext;
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public GetUserOrganisationInformationQueryHandler(
        IOrganizationRepository organizationRepository,
        IAccountUserContext loanUserContext,
        ILoanApplicationRepository loanApplicationRepository)
    {
        _loanUserContext = loanUserContext;
        _loanApplicationRepository = loanApplicationRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task<GetUserOrganisationInformationQueryResponse> Handle(GetUserOrganisationInformationQuery request, CancellationToken cancellationToken)
    {
        var account = await _loanUserContext.GetSelectedAccount();
        var organisationDetails = await _organizationRepository.GetBasicInformation(account, cancellationToken);
        var userDetails = await _loanUserContext.GetProfileDetails();

        var userLoanApplications = (await _loanApplicationRepository
                .LoadAllLoanApplications(account, cancellationToken))
            .OrderByDescending(application => application.CreatedOn ?? DateTime.MinValue)
            .ThenByDescending(x => x.LastModificationDate);

        return new GetUserOrganisationInformationQueryResponse(
            organisationDetails,
            userDetails.FirstName?.Value ?? string.Empty,
            account.Roles.All(r => r.Role == UserAccountRole.LimitedUser),
            userLoanApplications.ToList());
    }
}
