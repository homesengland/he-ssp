using HE.Investment.AHP.Contract.HomeTypes;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public record DesignFileParams(ApplicationId ApplicationId, HomeTypeId HomeTypeId);
