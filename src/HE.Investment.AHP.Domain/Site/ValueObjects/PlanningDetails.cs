using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class PlanningDetails : ValueObject, IQuestion
{
    public PlanningDetails(SitePlanningStatus planningStatus)
    {
        Build(planningStatus).CheckErrors();
    }

    public SitePlanningStatus PlanningStatus { get; private set; }

    public bool IsAnswered()
    {
        return false;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return PlanningStatus;
    }

    private OperationResult Build(SitePlanningStatus planningStatus)
    {
        var operationResult = OperationResult.New();

        PlanningStatus = EnumValidator<SitePlanningStatus>.Required(
            planningStatus,
            nameof(PlanningStatus),
            ValidationErrorMessage.MustBeSelected("planning status"),
            operationResult);

        return operationResult;
    }
}
