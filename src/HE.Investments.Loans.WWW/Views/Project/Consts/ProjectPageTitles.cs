namespace HE.Investments.Loans.WWW.Views.Project.Consts;

public static class ProjectPageTitles
{
    public const string StartPage = "Project details";

    public const string Name = "Add a projects";

    public const string StartDate = "Do you have an estimated build start date for this project?";

    public const string ManyHomes = "How many homes are you building?";

    public const string TypeHomes = "What type of homes are you building?";

    public const string ProjectType = "What land is this project going to be built on?";

    public const string PlanningReferenceNumberExists = "Do you have a planning reference number?";

    public const string PlanningReferenceNumber = "What is your planning reference number?";

    public const string PlanningPermissionStatus = "What is the status of your planning permission application for this project?";

    public const string Location = "How do you want to provide your project location?";

    public const string LocalAuthority = "Your local authority";

    public const string LocalAuthorityResult = "Select your local authority";

    public const string LocalAuthorityNoMatch = "The details you entered did not match our records";

    public const string LocalAuthorityConfirm = "Confirm your selection";

    public const string Ownership = "Do you have full ownership of this land?";

    public const string AdditionalDetails = "Additional land details";

    public const string GrantFundingExists = "To the best of your knowledge, has this land received public sector grant funding?";

    public const string GrantFunding = "Provide more information";

    public const string ChargesDebt = "To the best of your knowledge, are there any legal charges outstanding or debt secured on this land?";

    public const string AffordableHomes = "Is your project made up of more than 50% affordable housing?";

    public const string CheckAnswers = "Check your answers";

    public const string ReadOnlyAnswers = "Your answers";

    public static string Delete(string projectName) => $"Are you sure you want to remove {projectName}?";
}
