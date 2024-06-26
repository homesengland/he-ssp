namespace HE.Investment.AHP.WWW.Views.Delivery.Const;

public static class DeliveryPageTitles
{
    public const string LandingPage = "Delivery";

    public const string Name = "Name your delivery phase";

    public const string Details = "Delivery phase details";

    public const string BuildActivityType = "What is the build activity type?";

    public const string ReconfiguringExisting = "Are you reconfiguring existing residential properties to increase the number of homes?";

    public const string AcquisitionTranche = "Acquisition tranche";

    public const string StartOnSiteTranche = "Start on site tranche";

    public const string CompletionTranche = "Completion tranche";

    public const string AddHomes = "Add homes to this delivery phase";

    public const string List = "Delivery";

    public const string Complete = "Have you added all of the delivery phases for this scheme?";

    public const string AcquisitionMilestone = "Acquisition milestone";

    public const string StartOnSiteMilestone = "Start on site milestone";

    public const string PracticalCompletionMilestone = "Practical completion milestone";

    public const string UnregisteredBodyFollowUp = "Would you like to request additional payments for this phase?";

    public const string CheckAnswers = "Check your answers before adding a delivery phase";

    public const string Remove = "Are you sure you want to remove this delivery phase?";

    public static string SummaryOfDelivery(string deliveryPhaseName) => $"Summary of {deliveryPhaseName} and milestones";
}
