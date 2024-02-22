namespace HE.Investment.AHP.Contract.Delivery;

public enum DeliveryPhaseWorkflowState
{
    Create = 1,
    Name,
    TypeOfHomes,
    NewBuildActivityType,
    RehabBuildActivityType,
    ReconfiguringExisting,
    AddHomes,
    SummaryOfDelivery,
    SummaryOfDeliveryTranche,
    AcquisitionMilestone,
    StartOnSiteMilestone,
    PracticalCompletionMilestone,
    UnregisteredBodyFollowUp,
    CheckAnswers,
}
