using HE.Investment.AHP.Contract.Project;
using HE.Investments.AHP.Allocation.Contract.Project;
using HE.Investments.AHP.Allocation.Contract.Project.Queries;
using HE.Investments.AHP.Allocation.Domain.Project.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Config;
using HE.Investments.Programme.Contract.Queries;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Project.QueryHandlers;

public class GetProjectAllocationsQueryHandler : IRequestHandler<GetProjectAllocationsQuery, ProjectAllocationsModel>
{
    private readonly IProjectAllocationRepository _projectAllocationRepository;

    private readonly IMediator _mediator;

    private readonly IConsortiumUserContext _userContext;

    private readonly IProgrammeSettings _programmeSettings;

    public GetProjectAllocationsQueryHandler(
        IProjectAllocationRepository projectAllocationRepository,
        IMediator mediator,
        IConsortiumUserContext userContext,
        IProgrammeSettings programmeSettings)
    {
        _projectAllocationRepository = projectAllocationRepository;
        _mediator = mediator;
        _userContext = userContext;
        _programmeSettings = programmeSettings;
    }

    public async Task<ProjectAllocationsModel> Handle(GetProjectAllocationsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        var project = await _projectAllocationRepository.GetProjectAllocations(request.ProjectId, userAccount, cancellationToken);
        var programme = await _mediator.Send(new GetProgrammeQuery(ProgrammeId.From(_programmeSettings.AhpProgrammeId)), cancellationToken);

        var allocations = project.Allocations
            .OrderByDescending(x => x.LastModificationOn)
            .Select(x => new AllocationProjectModel(
                x.Id,
                x.Name,
                x.Tenure,
                x.HousesToDeliver,
                x.LocalAuthorityName))
            .ToList();

        return new ProjectAllocationsModel(
            project.Id,
            project.Name.Value,
            programme.Name,
            userAccount.Organisation!.RegisteredCompanyName,
            allocations,
            request.PaginationRequest.Page);
    }
}
