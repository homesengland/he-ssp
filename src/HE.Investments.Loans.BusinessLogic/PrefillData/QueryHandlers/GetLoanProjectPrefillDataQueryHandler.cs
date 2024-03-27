using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.PrefillData;
using HE.Investments.Loans.Contract.PrefillData.Queries;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.PrefillData.QueryHandlers;

public class GetLoanProjectPrefillDataQueryHandler : IRequestHandler<GetLoanProjectPrefillDataQuery, LoanProjectPrefillData?>
{
    private readonly ILoanPrefillDataRepository _prefillDataRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IApplicationProjectsRepository _projectsRepository;

    public GetLoanProjectPrefillDataQueryHandler(ILoanPrefillDataRepository prefillDataRepository, IAccountUserContext accountUserContext, IApplicationProjectsRepository projectsRepository)
    {
        _prefillDataRepository = prefillDataRepository;
        _accountUserContext = accountUserContext;
        _projectsRepository = projectsRepository;
    }

    public async Task<LoanProjectPrefillData?> Handle(GetLoanProjectPrefillDataQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var project = await _projectsRepository.GetByIdAsync(request.ProjectId, userAccount, ProjectFieldsSet.GetStatus, cancellationToken);
        if (project.FrontDoorSiteId.IsNotProvided())
        {
            return null;
        }

        var prefillData = await _prefillDataRepository.GetLoanProjectPrefillData(
            request.ApplicationId,
            project.FrontDoorSiteId!,
            userAccount,
            cancellationToken);

        return new LoanProjectPrefillData(prefillData.LocalAuthorityName);
    }
}
