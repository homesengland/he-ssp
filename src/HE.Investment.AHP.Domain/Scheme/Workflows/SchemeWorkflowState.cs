namespace HE.Investment.AHP.Domain.Scheme.Workflows;

public enum SchemeWorkflowState
{
    Start = 1,
    Funding,
    Affordability,
    SalesRisk,
    HousingNeeds,
    StakeholderDiscussions,
    CheckAnswers,
}
