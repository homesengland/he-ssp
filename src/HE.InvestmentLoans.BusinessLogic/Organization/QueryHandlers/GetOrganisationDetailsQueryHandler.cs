using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Organization;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;

public class GetOrganisationDetailsQueryHandler : IRequestHandler<GetOrganisationDetailsQuery, GetOrganisationDetailsQueryResponse>
{
    private readonly IOrganizationRepository _organizationRepository;

    private readonly ILoanUserContext _loanUserContext;

    public GetOrganisationDetailsQueryHandler(IOrganizationRepository organizationRepository, ILoanUserContext loanUserContext)
    {
        _organizationRepository = organizationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<GetOrganisationDetailsQueryResponse> Handle(GetOrganisationDetailsQuery request, CancellationToken cancellationToken)
    {
        var basicInformation = await _organizationRepository.GetBasicInformation(await _loanUserContext.GetSelectedAccount(), cancellationToken);
        var address = new List<string>()
        {
            basicInformation.Address.Line1,
            basicInformation.Address.Line2,
            basicInformation.Address.City,
            basicInformation.Address.PostalCode,
        };

        var organisationDataChengeRequestState = await _organizationRepository.GetOrganisationChangeRequestDetails(await _loanUserContext.GetSelectedAccount(), cancellationToken);

        return new GetOrganisationDetailsQueryResponse(
            new OrganisationDetailsViewModel(
                basicInformation.RegisteredCompanyName,
                basicInformation.ContactInformation.PhoneNUmber,
                address,
                basicInformation.CompanyRegistrationNumber,
                organisationDataChengeRequestState));
    }
}
