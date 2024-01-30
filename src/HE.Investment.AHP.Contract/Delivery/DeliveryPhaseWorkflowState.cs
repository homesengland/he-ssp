namespace HE.Investment.AHP.Contract.Delivery;

public enum DeliveryPhaseWorkflowState
{
    Start = 0,
    Create = 1,
    Name,
    TypeOfHomes,
    BuildActivityType,
    ReconfiguringExisting,
    AddHomes,
    SummaryOfDelivery,
    SummaryOfDeliveryEditable,
    SummaryOfDeliveryTranche,
    AcquisitionMilestone,
    StartOnSiteMilestone,
    PracticalCompletionMilestone,
    UnregisteredBodyFollowUp,
    CheckAnswers,
}
