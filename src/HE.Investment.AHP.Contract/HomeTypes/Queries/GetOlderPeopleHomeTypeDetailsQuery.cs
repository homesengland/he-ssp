namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetOlderPeopleHomeTypeDetailsQuery(string ApplicationId, string HomeTypeId)
    : GetHomeTypeSegmentQueryBase<OlderPeopleHomeTypeDetails>(ApplicationId, HomeTypeId);
