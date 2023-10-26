using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideLocationCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideLocationCommand, OperationResult>
{
    public ProvideLocationCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
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
                        throw new NotImplementedException($"Provided type of location: {request.TypeOfLocation} is incorrect. Available types: {ProjectFormOption.Coordinates}, {ProjectFormOption.LandRegistryTitleNumber}");
                }
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
