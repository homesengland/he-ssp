namespace HE.Investment.AHP.Contract.Application;

public enum ApplicationWorkflowState
{
    Start = 1,
    ApplicationsList,
    ApplicationName,
    ApplicationTenure,
    TaskList,
    OnHold,
    Reactivate,
    RequestToEdit,
    Withdraw,
    CheckAnswers,
    Submit,
    Completed,
}
