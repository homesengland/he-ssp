using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Documents;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public record DesignFileParams(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId) : IAhpFileParams
{
    public string PartitionId => ApplicationId.ToGuidAsString();
}
