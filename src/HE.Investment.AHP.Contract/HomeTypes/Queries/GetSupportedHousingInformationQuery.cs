namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetSupportedHousingInformationQuery(string ApplicationId, string HomeTypeId)
    : GetHomeTypeSegmentQueryBase<SupportedHousingInformation>(ApplicationId, HomeTypeId);
