using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using HE.Investments.FrontDoor.Domain.Services;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.QueryHandlers;

public class ValidateProjectAnswersQueryHandler : IRequestHandler<ValidateProjectAnswersQuery, (OperationResult OperationResult, ApplicationType ApplicationType)>
{
    private readonly IEligibilityService _eligibilityService;

    public ValidateProjectAnswersQueryHandler(IEligibilityService eligibilityService)
    {
        _eligibilityService = eligibilityService;
    }

    public async Task<(OperationResult OperationResult, ApplicationType ApplicationType)> Handle(ValidateProjectAnswersQuery request, CancellationToken cancellationToken)
    {
        return await _eligibilityService.GetEligibleApplication(request.ProjectId, cancellationToken);
    }
}
