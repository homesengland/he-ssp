using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.PrefillData.Data;
using HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.Contract.Projects.Queries;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using MediatR;

namespace HE.Investments.Loans.BusinessLogic.Projects.QueryHandlers;

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectViewModel>
{
    private readonly IApplicationProjectsRepository _applicationProjectsRepository;

    private readonly ILoanPrefillDataRepository _prefillDataRepository;

    private readonly IAccountUserContext _loanUserContext;

    public GetProjectQueryHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanPrefillDataRepository prefillDataRepository,
        IAccountUserContext loanUserContext)
    {
        _applicationProjectsRepository = applicationProjectsRepository;
        _prefillDataRepository = prefillDataRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<ProjectViewModel> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var project = await _applicationProjectsRepository.GetByIdAsync(request.ProjectId, userAccount, request.ProjectFieldsSet, cancellationToken);

        var prefillData = project.FrontDoorSiteId.IsProvided()
            ? await _prefillDataRepository.GetLoanProjectPrefillData(request.ApplicationId, project.FrontDoorSiteId!, userAccount, cancellationToken)
            : null;

        return ProjectMapper.MapToViewModel(project, request.ApplicationId, prefillData);
    }
}
