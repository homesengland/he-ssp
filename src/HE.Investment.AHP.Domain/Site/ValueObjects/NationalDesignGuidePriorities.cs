using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class NationalDesignGuidePriorities : ValueObject, IQuestion
{
    public NationalDesignGuidePriorities(IReadOnlyCollection<NationalDesignGuidePriority> priorities)
    {
        if (priorities.Any(x => x == NationalDesignGuidePriority.NoneOfTheAbove) && priorities.Count > 1)
        {
            OperationResult.New()
                .AddValidationError("DesignPriorities", ValidationErrorMessage.InvalidValue)
                .CheckErrors();
        }

        Values = priorities;
    }

    public NationalDesignGuidePriorities()
        : this(Array.Empty<NationalDesignGuidePriority>())
    {
    }

    public IEnumerable<NationalDesignGuidePriority> Values { get; }

    public bool IsAnswered()
    {
        return Values.Any();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Values;
    }
}
