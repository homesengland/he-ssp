using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class HousingNeeds : ValueObject
{
    public HousingNeeds(string? meetingLocalPriorities, string? meetingLocalHousingNeed)
    {
        Build(meetingLocalPriorities, meetingLocalHousingNeed).CheckErrors();
    }

    public string? MeetingLocalPriorities { get; private set; }

    public string? MeetingLocalHousingNeed { get; private set; }

    public void CheckIsComplete()
    {
        Build(MeetingLocalPriorities, MeetingLocalHousingNeed, true).CheckErrors();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return MeetingLocalPriorities;
        yield return MeetingLocalHousingNeed;
    }

    private OperationResult Build(string? meetingLocalPriorities, string? meetingLocalHousingNeed, bool isCompleteCheck = false)
    {
        var operationResult = OperationResult.New();

        MeetingLocalPriorities = Validator
            .For(meetingLocalPriorities, nameof(MeetingLocalPriorities), "Type and tenure of homes", operationResult)
            .IsProvidedIf(isCompleteCheck)
            .IsLongInput();

        MeetingLocalHousingNeed = Validator
            .For(meetingLocalHousingNeed, nameof(MeetingLocalHousingNeed), "Locally identified housing needs", operationResult)
            .IsProvidedIf(isCompleteCheck)
            .IsLongInput();

        return operationResult;
    }
}
