using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using MediatR;

namespace HE.Investments.FrontDoor.Domain.Project.CommandHandlers;

public class ProvideEnglandHousingDeliveryCommandHandler : IRequestHandler<ProvideEnglandHousingDeliveryCommand, OperationResult>
{
    private readonly IProjectRepository _projectRepository;

    private readonly IAccountUserContext _accountUserContext;

    public ProvideEnglandHousingDeliveryCommandHandler(IProjectRepository projectRepository, IAccountUserContext accountUserContext)
    {
        _projectRepository = projectRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<OperationResult> Handle(ProvideEnglandHousingDeliveryCommand request, CancellationToken cancellationToken)
    {
        if (request.ProjectId.IsNotProvided())
        {
            ProjectEntity.ValidateEnglandHousingDelivery(request.IsEnglandHousingDelivery);
        }
        else
        {
            var userAccount = await _accountUserContext.GetSelectedAccount();
            var project = await _projectRepository.GetProject(request.ProjectId!, userAccount, cancellationToken);

            project.ProvideIsEnglandHousingDelivery(request.IsEnglandHousingDelivery);

            await _projectRepository.Save(project, userAccount, cancellationToken);
        }

        return OperationResult.Success();
    }
}
