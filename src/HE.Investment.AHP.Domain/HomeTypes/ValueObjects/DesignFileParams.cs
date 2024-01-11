using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public record DesignFileParams(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId);
