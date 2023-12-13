using HE.Investment.AHP.Domain.Scheme.Constants;
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
            .For(meetingLocalPriorities, SchemeValidationFieldNames.MeetingLocalPriorities, operationResult)
            .IsProvidedIf(isCompleteCheck, "Type and tenure of homes are missing")
            .IsLongInput();

        MeetingLocalHousingNeed = Validator
            .For(meetingLocalHousingNeed, SchemeValidationFieldNames.MeetingLocalHousingNeed, operationResult)
            .IsProvidedIf(isCompleteCheck, "Locally identified housing needs are missing")
            .IsLongInput();

        return operationResult;
    }
}
