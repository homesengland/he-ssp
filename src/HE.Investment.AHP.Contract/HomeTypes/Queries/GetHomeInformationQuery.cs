namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeInformationQuery(string ApplicationId, string HomeTypeId) : GetHomeTypeSegmentQueryBase<HomeInformation>(ApplicationId, HomeTypeId);
