using HE.Investment.AHP.Contract.Scheme.Events;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using MediatR;

namespace HE.Investment.AHP.Domain.FinancialDetails.EventHandlers;

public class MarkFinancialDetailsAsInProgressEventHandler : IEventHandler<SchemeFundingHasBeenChangedEvent>
{
    private readonly IFinancialDetailsRepository _financialDetailsRepository;

    private readonly IAccountUserContext _accountUserContext;

    public MarkFinancialDetailsAsInProgressEventHandler(IFinancialDetailsRepository financialDetailsRepository, IAccountUserContext accountUserContext)
    {
        _financialDetailsRepository = financialDetailsRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task Handle(SchemeFundingHasBeenChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var financialDetails = await _financialDetailsRepository.GetById(domainEvent.ApplicationId, account, cancellationToken);
        if (financialDetails.SectionStatus.IsIn(SectionStatus.NotStarted))
        {
            return;
        }

        financialDetails.MarkAsNotCompleted();
        await _financialDetailsRepository.Save(financialDetails, account.SelectedOrganisationId(), cancellationToken);
    }
}
