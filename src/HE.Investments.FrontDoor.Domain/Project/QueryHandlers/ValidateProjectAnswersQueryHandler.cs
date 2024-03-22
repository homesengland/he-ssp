using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using HE.Investments.FrontDoor.Domain.Services;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.QueryHandlers;

public class ValidateProjectAnswersQueryHandler : IRequestHandler<ValidateProjectAnswersQuery, ApplicationType>
{
    private readonly IEligibilityService _eligibilityService;

    public ValidateProjectAnswersQueryHandler(IEligibilityService eligibilityService)
    {
        _eligibilityService = eligibilityService;
    }

    public async Task<ApplicationType> Handle(ValidateProjectAnswersQuery request, CancellationToken cancellationToken)
    {
        return await _eligibilityService.GetEligibleApplication(request.ProjectId, cancellationToken);
    }
}
