namespace HE.Investments.AHP.Consortium.Domain.Entities;

internal static class ConsortiumValidationErrors
{
    public const string IsAlreadyPartOfConsortium =
        "This organisation cannot be added to your consortium. Check you have selected the correct organisation. If it is correct, contact your Growth Manager";

    public const string RemoveConfirmationNotSelected = "Select whether you want to remove this organisation from consortium";
}
