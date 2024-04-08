using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.Application.QueryHandlers;

public class GetApplicationDetailsQueryHandler : IRequestHandler<GetApplicationDetailsQuery, ApplicationExtendedDetails>
{
    private readonly IApplicationRepository _repository;

    private readonly IAccountUserContext _accountUserContext;

    public GetApplicationDetailsQueryHandler(IApplicationRepository repository, IAccountUserContext accountUserContext)
    {
        _repository = repository;
        _accountUserContext = accountUserContext;
    }

    public async Task<ApplicationExtendedDetails> Handle(GetApplicationDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _repository.GetApplicationWithFundingDetailsById(request.ApplicationId, account, cancellationToken);

        return new ApplicationExtendedDetails(
            application.SiteId,
            application.ApplicationId,
            application.ApplicationName,
            application.ReferenceNumber,
            application.Tenure.ToString(),
            application.HousesToDeliver,
            application.RequiredFunding,
            application.TotalSchemeCost(),
            application.RepresentationsAndWarranties);
    }
}
