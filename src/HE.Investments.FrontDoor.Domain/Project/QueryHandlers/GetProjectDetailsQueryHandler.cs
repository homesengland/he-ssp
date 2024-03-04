using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.QueryHandlers;

public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, ProjectDetails>
{
    public Task<ProjectDetails> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ProjectDetails(request.ProjectId, "Test name", true));
    }
}
