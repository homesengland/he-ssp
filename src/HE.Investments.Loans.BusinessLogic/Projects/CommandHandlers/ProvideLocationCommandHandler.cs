using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Loans.BusinessLogic.Projects.CommandHandlers;

public class ProvideLocationCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideLocationCommand, OperationResult>
{
    public ProvideLocationCommandHandler(
        IApplicationProjectsRepository applicationProjectsRepository,
        ILoanApplicationRepository loanApplicationRepository,
        IAccountUserContext loanUserContext,
        ILogger<ProjectCommandHandlerBase> logger)
        : base(applicationProjectsRepository, loanApplicationRepository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideLocationCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.TypeOfLocation.IsNotProvided())
                {
                    return;
                }

                switch (request.TypeOfLocation)
                {
                    case ProjectFormOption.Coordinates:
                        project.ProvideCoordinates(new Coordinates(request.Coordinates));
                        break;
                    case ProjectFormOption.LandRegistryTitleNumber:
                        project.ProvideLandRegistryNumber(new LandRegistryTitleNumber(request.LandregistryTitleNumber));
                        break;
                    default:
                        throw new NotImplementedException(
                            $"Provided type of location: {request.TypeOfLocation} is incorrect. Available types: {ProjectFormOption.Coordinates}, {ProjectFormOption.LandRegistryTitleNumber}");
                }
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
