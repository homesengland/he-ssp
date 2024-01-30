using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Constants;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Site.ValueObjects;

public class NationalDesignGuidePriorities : ValueObject, IQuestion
{
    public NationalDesignGuidePriorities(IEnumerable<NationalDesignGuidePriority> priorities)
    {
        if (priorities == null || !priorities.Any())
        {
            OperationResult.New()
                .AddValidationError(SiteValidationFieldNames.DesignPriorities, ValidationErrorMessage.MissingRequiredField("National Design Guide"))
                .CheckErrors();
        }

        Values = priorities!;
    }

    public NationalDesignGuidePriorities()
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
