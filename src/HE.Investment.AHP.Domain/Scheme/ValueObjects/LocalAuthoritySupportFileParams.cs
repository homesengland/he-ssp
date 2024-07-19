using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Documents;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public record LocalAuthoritySupportFileParams(AhpApplicationId ApplicationId) : IAhpFileParams
{
    public string PartitionId => ApplicationId.ToGuidAsString();
}
