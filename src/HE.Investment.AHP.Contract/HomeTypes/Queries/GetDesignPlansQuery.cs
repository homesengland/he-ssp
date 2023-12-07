namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetDesignPlansQuery(string ApplicationId, string HomeTypeId) : GetHomeTypeSegmentQueryBase<DesignPlans>(ApplicationId, HomeTypeId);
