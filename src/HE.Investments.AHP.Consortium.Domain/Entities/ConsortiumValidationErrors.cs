namespace HE.Investments.AHP.Consortium.Domain.Entities;

internal static class ConsortiumValidationErrors
{
    public const string IsPartOfThisConsortium = "The organisation you are trying to add is already added or being added as a member of this consortium";

    public const string IsPartOfOtherConsortium = "The organisation you are trying to add is already added or being added to another consortium";

    public const string IsSitePartner = "This organisation is a Site Partner";

    public const string IsApplicationPartner = "This organisation is an Application Partner";

    public const string RemoveConfirmationNotSelected = "Select yes if you want to remove the organisation";
}
