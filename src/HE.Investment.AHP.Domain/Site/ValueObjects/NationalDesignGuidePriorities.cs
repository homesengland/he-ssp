using HE.Investment.AHP.Contract.Site.Constants;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class NationalDesignGuidePriorities : ValueObject, IQuestion
{
    public NationalDesignGuidePriorities(IReadOnlyCollection<NationalDesignGuidePriority> priorities)
    {
        if (!priorities.Any())
        {
            OperationResult.New()
                .AddValidationError(SiteValidationFieldNames.DesignPriorities, "You need to choose at least one option from National Design Guide")
                .CheckErrors();
        }

        if (priorities.Any(x => x == NationalDesignGuidePriority.NoneOfTheAbove) && priorities.Count > 1)
        {
            OperationResult.New()
                .AddValidationError(SiteValidationFieldNames.DesignPriorities, "Invalid values where provided for National Design Guide priorities")
                .CheckErrors();
        }

        Values = priorities;
    }

    public NationalDesignGuidePriorities()
    {
        Values = Enumerable.Empty<NationalDesignGuidePriority>();
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
