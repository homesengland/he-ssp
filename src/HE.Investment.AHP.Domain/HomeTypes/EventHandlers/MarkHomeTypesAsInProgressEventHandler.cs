using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Contract.Scheme.Events;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;

namespace HE.Investment.AHP.Domain.HomeTypes.EventHandlers;

public class MarkHomeTypesAsInProgressEventHandler :
    IEventHandler<HomeTypeHasBeenCreatedEvent>,
    IEventHandler<HomeTypeHasBeenUpdatedEvent>,
    IEventHandler<HomeTypeHasBeenRemovedEvent>,
    IEventHandler<SchemeNumberOfHomesHasBeenUpdatedEvent>
{
    private readonly IApplicationRepository _applicationRepository;

    private readonly IApplicationSectionStatusChanger _sectionStatusChanger;

    private readonly IAccountUserContext _accountUserContext;

    public MarkHomeTypesAsInProgressEventHandler(
        IApplicationRepository applicationRepository,
        IApplicationSectionStatusChanger sectionStatusChanger,
        IAccountUserContext accountUserContext)
    {
        _applicationRepository = applicationRepository;
        _sectionStatusChanger = sectionStatusChanger;
        _accountUserContext = accountUserContext;
    }

    public async Task Handle(HomeTypeHasBeenCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    public async Task Handle(HomeTypeHasBeenUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    public async Task Handle(HomeTypeHasBeenRemovedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken);
    }

    public async Task Handle(SchemeNumberOfHomesHasBeenUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        await ChangeStatus(domainEvent.ApplicationId, cancellationToken, preventStarting: true);
    }

    private async Task ChangeStatus(AhpApplicationId applicationId, CancellationToken cancellationToken, bool preventStarting = false)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var application = await _applicationRepository.GetApplicationBasicInfo(applicationId, account, cancellationToken);
        if (preventStarting && application.Sections.HomeTypesStatus.IsIn(SectionStatus.NotStarted))
        {
            return;
        }

        await _sectionStatusChanger.ChangeSectionStatus(
            applicationId,
            account.SelectedOrganisationId(),
            SectionType.HomeTypes,
            SectionStatus.InProgress,
            cancellationToken);
    }
}
