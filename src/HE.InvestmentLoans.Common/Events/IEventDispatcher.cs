using HE.InvestmentLoans.Common.Domain;

namespace HE.InvestmentLoans.Common.Events;

public interface IEventDispatcher
{
    Task Publish(DomainEntity domainEntity, CancellationToken cancellationToken);
}
