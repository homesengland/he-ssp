namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetModernMethodsConstructionQuery(string ApplicationId, string HomeTypeId)
    : GetHomeTypeSegmentQueryBase<ModernMethodsConstruction>(ApplicationId, HomeTypeId);
