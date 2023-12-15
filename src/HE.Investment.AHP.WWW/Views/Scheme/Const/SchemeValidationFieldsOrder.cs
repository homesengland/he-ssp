using HE.Investment.AHP.Contract.Scheme.Constants;

namespace HE.Investment.AHP.WWW.Views.Scheme.Const;
public static class SchemeValidationFieldsOrder
{
    public static List<string> Affordability => new() { SchemeValidationFieldNames.AffordabilityEvidence };

    public static List<string> Funding => new()
    {
        SchemeValidationFieldNames.RequiredFunding,
        SchemeValidationFieldNames.HousesToDeliver,
    };

    public static List<string> HousingNeeds => new()
    {
        SchemeValidationFieldNames.MeetingLocalPriorities,
        SchemeValidationFieldNames.MeetingLocalHousingNeed,
    };

    public static List<string> SalesRisk => new() { SchemeValidationFieldNames.SalesRisk };

    public static List<string> StakeholderDiscussions => new()
    {
        SchemeValidationFieldNames.StakeholderDiscussionsReport,
        SchemeValidationFieldNames.StakeholderDiscussionFile,
    };
}
