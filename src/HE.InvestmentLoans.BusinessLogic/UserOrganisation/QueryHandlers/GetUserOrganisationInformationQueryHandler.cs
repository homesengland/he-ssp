using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Contract.UserOrganisation.Queries;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.UserOrganisation.QueryHandlers;

public class GetUserOrganisationInformationQueryHandler : IRequestHandler<GetUserOrganisationInformationQuery, GetUserOrganisationInformationQueryResponse>
{
    private readonly ILoanUserContext _loanUserContext;
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly ILoanUserRepository _loanUserRepository;

    public GetUserOrganisationInformationQueryHandler(
        IOrganizationRepository organizationRepository,
        ILoanUserRepository loanUserRepository,
        ILoanUserContext loanUserContext,
        ILoanApplicationRepository loanApplicationRepository)
    {
        _loanUserContext = loanUserContext;
        _loanApplicationRepository = loanApplicationRepository;
        _organizationRepository = organizationRepository;
        _loanUserRepository = loanUserRepository;
    }

    public async Task<GetUserOrganisationInformationQueryResponse> Handle(GetUserOrganisationInformationQuery request, CancellationToken cancellationToken)
    {
        var account = await _loanUserContext.GetSelectedAccount();
        var organisationDetails = await _organizationRepository.GetBasicInformation(account, cancellationToken);
        var userDetails = await _loanUserRepository.GetUserDetails(_loanUserContext.UserGlobalId);

        var userLoanApplications = (await _loanApplicationRepository
            .LoadAllLoanApplications(account, cancellationToken))
            .OrderByDescending(application => application.LastModificationDate ?? DateTime.MinValue);

        return new GetUserOrganisationInformationQueryResponse(
            organisationDetails,
            userDetails.FirstName!,
            account.Roles.All(r => r.Role == UserAccountRole.LimitedUser),
            userLoanApplications.ToList());
    }
}
