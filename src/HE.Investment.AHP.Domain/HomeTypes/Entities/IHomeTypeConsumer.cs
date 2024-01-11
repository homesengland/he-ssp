using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeConsumer
{
    string HomeTypeConsumerName { get; }

    bool IsHomeTypeUsed(HomeTypeId homeTypeId);
}
