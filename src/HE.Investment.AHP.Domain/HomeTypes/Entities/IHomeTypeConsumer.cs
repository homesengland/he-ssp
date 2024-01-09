using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public interface IHomeTypeConsumer
{
    string HomeTypeConsumerName { get; }

    bool IsHomeTypeUsed(HomeTypeId homeTypeId);
}
