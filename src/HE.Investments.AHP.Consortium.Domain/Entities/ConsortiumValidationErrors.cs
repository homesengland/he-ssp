namespace HE.Investments.AHP.Consortium.Domain.Entities;

internal static class ConsortiumValidationErrors
{
    public const string IsPartOfThisConsortium = "The organisation you are trying to add is already added or being added as a member of this consortium";

    public const string IsPartOfOtherConsortium = "The organisation you are trying to add is already added or being added to another consortium";

    public const string RemoveConfirmationNotSelected = "Select whether you want to remove this organisation from consortium";
}
