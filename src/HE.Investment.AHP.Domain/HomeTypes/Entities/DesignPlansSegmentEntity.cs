using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.DesignPlans)]
public class DesignPlansSegmentEntity : IHomeTypeSegmentEntity
{
    private readonly List<HappiDesignPrincipleType> _designPrinciples;

    public DesignPlansSegmentEntity(IEnumerable<HappiDesignPrincipleType>? designPrinciples = null)
    {
        _designPrinciples = designPrinciples?.ToList() ?? new List<HappiDesignPrincipleType>();
    }

    public IReadOnlyCollection<HappiDesignPrincipleType> DesignPrinciples => _designPrinciples;

    public void ChangeDesignPrinciples(IEnumerable<HappiDesignPrincipleType> designPrinciples)
    {
        var uniquePrinciples = designPrinciples.Distinct().ToList();
        if (uniquePrinciples.Contains(HappiDesignPrincipleType.NoneOfThese) && uniquePrinciples.Count > 1)
        {
            OperationResult.New()
                .AddValidationError(
                    nameof(DesignPrinciples),
                    ValidationErrorMessage.ExclusiveOptionSelected("design principle", HappiDesignPrincipleType.NoneOfThese.GetDescription()))
                .CheckErrors();
        }

        _designPrinciples.Clear();
        _designPrinciples.AddRange(uniquePrinciples);
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new DesignPlansSegmentEntity(DesignPrinciples);
    }

    public bool IsCompleted()
    {
        return DesignPrinciples.Any();
    }
}
